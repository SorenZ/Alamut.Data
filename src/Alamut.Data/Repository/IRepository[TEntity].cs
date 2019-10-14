using Alamut.Data.Entity;

namespace Alamut.Data.Repository
{
    /// <inheritdoc />
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : IEntity
    { }
}