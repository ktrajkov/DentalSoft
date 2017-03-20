namespace DentalSoft.Data.Repository.Interfaces
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
