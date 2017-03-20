namespace DentalSoft.Web.Controllers.Base
{
    using DentalSoft.Common.Extensions;
    using DentalSoft.Data.Contracts;
    using DentalSoft.Data.Repository.Interfaces;
    using DentalSoft.Data.Services;
    using DentalSoft.Data.Services.Interfaces;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class EntityController<TContract, TEntity, TFilter> : BaseController
        where TEntity : class
        where TContract : PresentationModel
    {
        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="request">The DataSource request.</param>
        /// <param name="contract">The model.</param>
        /// <returns>A JSON result.</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Create([DataSourceRequest] DataSourceRequest request, TContract contract)
        {
            return this.SaveEntity(request, contract);
        }
        /// <summary>
        /// Gets a list of models and the total count according to the filter specified.
        /// </summary>
        /// <param name="request">The DataSource request.</param>
        /// <param name="inputFilter">The filter.</param>
        /// <returns>A JSON result.</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Read([DataSourceRequest] DataSourceRequest request, TFilter inputFilter)
        {
            var repository = RepositoryManager.GetRepository<TContract, TEntity>();
            var result = repository.AllToModel<TFilter>(inputFilter).ToDataSourceResult(request);
            return base.JsonNet(result);
        }
        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="request">The DataSource request.</param>
        /// <param name="contract">The model.</param>
        /// <returns>A JSON result.</returns>  
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Update([DataSourceRequest] DataSourceRequest request, TContract contract)
        {
            return this.SaveEntity(request, contract);
        }
        /// <summary>
        /// Deletes model.
        /// </summary>
        /// <param name="request">The DataSource request.</param>
        /// <param name="contract">The model to be deleted.</param>
        /// <returns>A JSON result.</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Delete([DataSourceRequest] DataSourceRequest request, TContract contract)
        {
            if (contract != null)
            {
                this.DeleteEntity(contract.Id);
            }
            DataSourceResult data = QueryableExtensions.ToDataSourceResult(new TContract[]
            {
               contract
            }, request, base.ModelState);
            return base.JsonNet(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult List(TFilter entityFilter, string format = null, string sortExpression = null, bool fullContract = false)
        {
            IEnumerable<TContract> enumerable = this.ListEntities(entityFilter, sortExpression);
            object data;
            if (fullContract || !typeof(System.IFormattable).IsAssignableFrom(typeof(TContract)))
            {
                data = enumerable;
            }
            else
            {
                data = this.FormatList(enumerable, format);
            }
            return base.JsonNet(data);
        }

        #region Protected Members

        /// <summary>
        /// Performs the save of the model.
        /// </summary>
        /// <param name="contract">The model to be saved.</param>
        /// <returns>The already saved model.</returns>
        protected virtual TContract SaveEntity(TContract contract)
        {
            IRepository<TContract, TEntity> repository = RepositoryManager.GetRepository<TContract, TEntity>();
            repository.Save(contract);
            RepositoryManager.Commit();
            return repository.Load();
        }

        protected virtual void DeleteEntity(object id)
        {
            IRepository<TContract, TEntity> repository = RepositoryManager.GetRepository<TContract, TEntity>();
            repository.Delete(id);
        }

        protected virtual IEnumerable<TContract> ListEntities(TFilter entityFilter, string sortExpression)
        {
            var repository = RepositoryManager.GetRepository<TContract, TEntity>();
            return repository.AllToModel<TFilter>(entityFilter).OrderBy(sortExpression).ToList();
        }
        /// <summary>
        /// Build the result to be sent to the client.
        /// </summary>
        /// <param name="items">A list of modles.</param>
        /// <param name="format">The format to be applied in the data transformation.</param>
        /// <returns>A list of data objects.</returns>
        protected virtual IEnumerable<object> FormatList(IEnumerable<TContract> items, string format)
        {
            return (
                from item in items
                select new
                {
                    Text = ((System.IFormattable)((object)item)).ToString(format, CultureInfo.CurrentCulture),
                    Value = item.Id
                }).ToList();
        }

        #endregion

        #region Private Members

        private ActionResult SaveEntity(DataSourceRequest request, TContract contract)
        {
            TContract tContract = default(TContract);
            if (contract != null && ModelState.IsValid)
            {
                tContract = this.SaveEntity(contract);
            }

            DataSourceResult data = QueryableExtensions.ToDataSourceResult(new TContract[]
			{
				tContract
			}, request, ModelState);
            return base.JsonNet(data);
        }

        #endregion
    }
}