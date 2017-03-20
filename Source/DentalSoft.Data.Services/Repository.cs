namespace DentalSoft.Data.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DentalSoft.Data.Contracts;
    using DentalSoft.Data.Models;
    using DentalSoft.Data.Repository.Base;
    using DentalSoft.Data.Repository.Interfaces;
    using DentalSoft.Data.Services.Interfaces;
    using System.Linq;
    using System.Web.Mvc;

    //TODO: Fix check for null
    public class Repository<TContract, TEntity> : DeletableRepository<TEntity>, IRepository<TContract, TEntity>
        where TContract : PresentationModel
        where TEntity : AuditInfo, IDeletableEntity
    {
        //TODO: Addreess not null for new entity of patient
        public TContract GetByIdToModel(object id)
        {
            var entity = base.GetById(id);
            var contract = Mapper.Map<TContract>(entity);
            var contractFilledHandler = (IContractFilled<TEntity, TContract>)DependencyResolver.Current
                .GetService(typeof(IContractFilled<TEntity, TContract>));
            if (contractFilledHandler != null)
            {
                contractFilledHandler.ContractFilled(entity, contract);
            }
            return contract;
        }

        public TContract Load()
        {
            var id = this.entity.Id;
            base.Detach(this.entity);
            return this.GetByIdToModel(id);
        }

        public void Save(TContract contract)
        {
            TEntity entity;
            if (contract.Id == null || contract.Id == 0)
            {
                entity = Mapper.Map<TContract, TEntity>(contract);
            }
            else
            {
                entity = base.GetById(contract.Id);
                var entityFillingHandler = (IEntityFilling<TContract, TEntity>)DependencyResolver.Current
                     .GetService(typeof(IEntityFilling<TContract, TEntity>));
                if (entityFillingHandler != null)
                {
                    entityFillingHandler.Filling(contract, entity);

                }

                Mapper.Map<TContract, TEntity>(contract, entity);
            }

            var checkForUniqunessHandler = (IEntityCheckForUniqueness<TEntity>)DependencyResolver.Current
                .GetService(typeof(IEntityCheckForUniqueness<TEntity>));
            if (checkForUniqunessHandler != null)
            {
                checkForUniqunessHandler.CheckForUniqueness(entity);

            }

            var entityFilledHandler = (IEntityFilled<TContract, TEntity>)DependencyResolver.Current
              .GetService(typeof(IEntityFilled<TContract, TEntity>));
            if (entityFilledHandler != null)
            {
                entityFilledHandler.Filled(contract, entity);

            }
            if (entity.Id > 0)
            {
                base.Update(entity);
            }
            else
            {
                base.Add(entity);
            }

            var entitySavedHandler = (IEntitySaved<TEntity>)DependencyResolver.Current
             .GetService(typeof(IEntitySaved<TEntity>));
            if (entitySavedHandler != null)
            {
                entitySavedHandler.Saved(entity);

            }

            this.entity = entity;
        }

        public IQueryable<TContract> AllToModel()
        {
            return base.All().Project().To<TContract>();
        }

        public IQueryable<TContract> AllToModel<TFilter>(TFilter filter)
        {
            return base.All<TFilter>(filter).Project().To<TContract>();
        }

        public IQueryable<TContract> AllWithDeletedToModel()
        {
            return base.AllWithDeleted().Project().To<TContract>();
        }

        #region Private Members
        private TEntity entity;
        #endregion


        public void Delete(TContract contract)
        {
            TEntity entity = base.GetById(contract.Id);
            var entityDeletingHandler = (IEntityDeleting<TEntity>)DependencyResolver.Current
                 .GetService(typeof(IEntityDeleting<TEntity>));
            if (entityDeletingHandler != null)
            {
                entityDeletingHandler.Deleting(entity);
            }

            base.Delete(entity);

            var entityDeletedHandler = (IEntityDeleted<TEntity>)DependencyResolver.Current
                .GetService(typeof(IEntityDeleted<TEntity>));
            if (entityDeletedHandler != null)
            {
                entityDeletedHandler.Deleted(entity);

            }
        }
    }
}
