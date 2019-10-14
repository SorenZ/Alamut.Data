using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alamut.Data.Sql
{
    /// <summary>
    /// Represents a unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken = default);
        void RollBack();
        Task RollBackAsync(CancellationToken cancellationToken = default);
    }
}
