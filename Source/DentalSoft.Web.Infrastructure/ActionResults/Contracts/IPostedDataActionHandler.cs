namespace DentalSoft.Web.Infrastructure.ActionResults.Contracts
{
    public interface IPostedDataActionHandler<TContract,TEntity>
    {
        void Handle(TContract model);
    }
}
    