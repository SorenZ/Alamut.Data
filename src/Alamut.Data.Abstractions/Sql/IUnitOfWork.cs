using System;
using System.Threading.Tasks;

namespace Alamut.Data.Abstractions.Sql
{
    /// <summary>
    /// Represents a unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Task CommitAsync();
        void RollBack();
        Task RollBackAsync();
    }
}
