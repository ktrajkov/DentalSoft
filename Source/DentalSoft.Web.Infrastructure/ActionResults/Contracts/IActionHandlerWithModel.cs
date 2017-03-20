namespace DentalSoft.Web.Infrastructure.ActionResults.Contracts
{
    public interface IActionHandlerWithModel<TContract,TEntity>
    {
        TContract Handle();
    }
}
