using System;
using System.Threading;
using System.Threading.Tasks;
using Alamut.Abstractions.Structure;

namespace Alamut.Data.Sql
{
    /// <summary>
    /// Represents a unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        Result Commit();
        Task<Result> CommitAsync(CancellationToken cancellationToken = default);
        void RejectChanges();
    }
}
