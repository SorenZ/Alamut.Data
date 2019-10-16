using Alamut.Data.Entity;

namespace Alamut.Data.Repository
{
    public interface ISmartRepository<TEntity> : ISmartRepository<TEntity, int> where TEntity : IEntity
    { }
}