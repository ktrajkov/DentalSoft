namespace DentalSoft.Common.Filters
{
    using System;

    /// <summary>
    /// The base class for all filter attribute applied to a property or field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class AssociationFilterAttribute : Attribute
    {
    }
}
