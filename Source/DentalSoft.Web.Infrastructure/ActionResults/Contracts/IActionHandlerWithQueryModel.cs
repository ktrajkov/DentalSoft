namespace DentalSoft.Web.Infrastructure.ActionResults.Contracts
{
    using System.Collections.Generic;

    public interface IActionHandlerWithQueryModel<TContract, TEntity>
    {
        IEnumerable<TContract> Handle();
    }
}
