namespace DentalSoft.Services.Operations
{
    using DentalSoft.Data.Models.DailyPlannings;
    using DentalSoft.Data.Models.Operation;
    using DentalSoft.Data.Models.Treatments;
    using DentalSoft.Data.Services;
    using DentalSoft.Data.Services.Interfaces;
using DentalSoft.Services.DailyPlannings;
    using System.Diagnostics;

    public class OperationEntitySaved : IEntitySaved<Operation>
    {
      public OperationEntitySaved(IMedicalCheckupCreator medicalCheckupCreator)
        {
            this.medicalCheckupCreator = medicalCheckupCreator;
        }
        public void Saved(Operation entity)
        {         
            medicalCheckupCreator.CheckAndCreate(entity);         
        }

        #region Private members
        private IMedicalCheckupCreator medicalCheckupCreator;
        #endregion

    }
}
