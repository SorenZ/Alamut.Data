using Alamut.Data.Abstractions.Entity;

namespace Alamut.Data.Abstractions.Repository
{
    /// <inheritdoc />
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : IEntity
    { }
}