
namespace DentalSoft.Web.Infrastructure.ActionResults.Contracts
{
    public interface IActionHandlerById<TContract, TEntity>
    {
        TContract Handle(int? id);
    }
}
