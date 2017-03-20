namespace DentalSoft.Web.Infrastructure.Contracts.Validators
{
    using System.Web.Mvc;

    public interface IValidator<T>
    {
        bool Validate(T model, ControllerContext controllerContext);
    }
}
