namespace DentalSoft.Common.Extensions
{
    using DentalSoft.Common.Filters;
    using DentalSoft.Common.Mapping;
    using LinqKit;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class IQueryableExtensions
    {
        public static IQueryable<TEntity> Filter<TEntity, TFilter>(this IQueryable<TEntity> query, TFilter filter)
        {
            var filterProperties = typeof(TFilter).GetProperties().Where(x => x.GetValue(filter) != null);
            if (filterProperties.Count() > 0)
            {
                var predicate = PredicateBuilder.False<TEntity>();
                var matchPropCount = 0;
                foreach (var filterProperty in filterProperties)
                {
                    matchPropCount++;
                    object filterValue;
                    if (filterProperty.PropertyType.BaseType == typeof(Enum))
                    {
                        filterValue = (int)Enum.Parse(filterProperty.PropertyType, filterProperty.GetValue(filter).ToString());
                    }
                    else
                    {
                        filterValue = filterProperty.GetValue(filter);
                    }

                    if (matchPropCount > 1)
                    {
                        predicate = predicate.And(CreateLike<TEntity>(filterProperty, filterValue));
                    }
                    else
                    {
                        predicate = predicate.Or(CreateLike<TEntity>(filterProperty, filterValue));
                    }

                }
                return query.AsExpandable().Where(predicate);
            }
            else
            {
                return query;
            }
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortExpression)
        {
            if (source == null)
                throw new ArgumentNullException("source", "source is null.");

            if (!string.IsNullOrEmpty(sortExpression))
            {
                var parts = sortExpression.Split(' ');
                var isDescending = false;
                var propertyName = "";
                var tType = typeof(T);

                if (parts.Length > 0 && parts[0] != "")
                {
                    propertyName = parts[0];

                    if (parts.Length > 1)
                    {
                        isDescending = parts[1].ToLower().Contains("esc");
                    }

                    PropertyInfo prop = tType.GetProperty(propertyName);

                    if (prop == null)
                    {
                        throw new ArgumentException(string.Format("No property '{0}' on type '{1}'", propertyName, tType.Name));
                    }

                    var funcType = typeof(Func<,>)
                        .MakeGenericType(tType, prop.PropertyType);

                    var lambdaBuilder = typeof(Expression)
                        .GetMethods()
                        .First(x => x.Name == "Lambda" && x.ContainsGenericParameters && x.GetParameters().Length == 2)
                        .MakeGenericMethod(funcType);

                    var parameter = Expression.Parameter(tType);
                    var propExpress = Expression.Property(parameter, prop);

                    var sortLambda = lambdaBuilder
                        .Invoke(null, new object[] { propExpress, new ParameterExpression[] { parameter } });

                    var sorter = typeof(Queryable)
                        .GetMethods()
                        .FirstOrDefault(x => x.Name == (isDescending ? "OrderByDescending" : "OrderBy") && x.GetParameters().Length == 2)
                        .MakeGenericMethod(new[] { tType, prop.PropertyType });

                    return (IQueryable<T>)sorter
                        .Invoke(null, new object[] { source, sortLambda });
                }
            }
            return source;
        }

        #region Private Members

        private static Expression<Func<TEntity, bool>> CreateLike<TEntity>(PropertyInfo filterProperty, object value)
        {
            var filterType = filterProperty.PropertyType;
            var associationPath = GetAssociationPath(filterProperty);

            var baseExpression = Expression.Parameter(typeof(TEntity), "f");
            var nestedExpression = NestedExpressionProperty(baseExpression, associationPath);

            var filterValue = Expression.Constant(value);           

            Expression body;
            var associationFilterAttributes = filterProperty.GetCustomAttributes(typeof(AssociationFilterAttribute), true);
            if (associationFilterAttributes.Length > 0)
            {
                ExpressionType operation;
                var attribute = associationFilterAttributes[0].GetType();
                if (attribute == typeof(InAttribute))
                {                                       
                    body = Expression.Call(typeof(Enumerable), "Contains", new[] { nestedExpression.Type }, filterValue, nestedExpression);
                }
                else
                {
                    if (!operationMap.TryGetValue(attribute.Name, out operation))
                    {
                        throw new ArgumentOutOfRangeException("FilterOper", attribute.Name, "Invalid filter operation");
                    }

                    body = MakeBinaryExpressionWithConverter(operation, nestedExpression, filterValue);
                }
            }
            else if (filterType.FullName == "System.String")
            {
                MethodInfo method = filterType.GetMethod("Contains", new[] { filterType });
                body = Expression.Call(nestedExpression, method, filterValue);
            }
            else
            {
                body = MakeBinaryExpressionWithConverter(ExpressionType.Equal, nestedExpression, filterValue);
            }

            return Expression.Lambda<Func<TEntity, bool>>(body, baseExpression);
        }

        private static MemberExpression NestedExpressionProperty(Expression expression, string propertyName)
        {
            string[] parts = propertyName.Split('.');
            int partsL = parts.Length;

            return (partsL > 1)
                ?
                Expression.Property(
                    NestedExpressionProperty(
                        expression,
                        parts.Take(partsL - 1)
                            .Aggregate((a, i) => a + "." + i)
                    ),
                    parts[partsL - 1]) : Expression.Property(expression, propertyName);
        }

        private static PropertyInfo GetProperty(String propName, Type type)
        {
            var parts = propName.Split('.').ToList();
            var currentPart = parts[0];
            PropertyInfo info = type.GetProperty(currentPart);
            if (info == null) { return null; }
            if (propName.IndexOf(".") > -1)
            {
                parts.Remove(currentPart);
                return GetProperty(String.Join(".", parts), info.PropertyType);
            }
            else
            {
                return info;
            }
        }

        private static string GetAssociationPath(PropertyInfo filterProperty)
        {
            var filterAttr = (MapAssociationAttribute)filterProperty.GetCustomAttributes(typeof(MapAssociationAttribute)).FirstOrDefault();
            return filterAttr != null ? filterAttr.AssociationPath : filterProperty.Name;

        }

        private static BinaryExpression MakeBinaryExpressionWithConverter(ExpressionType binaryType, Expression left, Expression right)
        {
            var rightConverted = right.Type != left.Type ? (Expression)Expression.Convert(right, left.Type) : (Expression)right;
            return Expression.MakeBinary(binaryType, left, rightConverted);
        }

        private static Dictionary<string, ExpressionType> operationMap = new Dictionary<string, ExpressionType>
        {
            { "EqualAttribute",                ExpressionType.Equal },
            { "NotEqualAttribute",             ExpressionType.NotEqual },
            { "LessThanAttribute",             ExpressionType.LessThan },
            { "LessThanOrEqualAttribute",      ExpressionType.LessThanOrEqual },
            { "GreaterThanAttribute",          ExpressionType.GreaterThan },
            { "GreaterThanOrEqualAttribute",   ExpressionType.GreaterThanOrEqual },
        };

        #endregion
    }


}

