namespace DentalSoft.Web.Validators.Base
{
    using System;
    using System.Web.Mvc;
    using Infrastructure.Validators;

    public abstract class BaseValidator<T> : IValidator<T>
    {
        public abstract bool Validate(T model, ControllerContext controllerContext);

        protected bool TryValidateModel(object model, ControllerContext controllerContext)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            var modelState = controllerContext.Controller.ViewData.ModelState;
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());

            foreach (var validationResult in ModelValidator.GetModelValidator(metadata, controllerContext).Validate(null))
            {
                modelState.AddModelError(validationResult.MemberName, validationResult.Message);
            }

            return modelState.IsValid;
        }
    }
}
