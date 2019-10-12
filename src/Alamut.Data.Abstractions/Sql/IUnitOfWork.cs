using System;

namespace Alamut.Data.Abstractions.Sql
{
    /// <summary>
    /// Represents a unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void RollBack();
    }
}
