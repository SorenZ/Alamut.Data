using Alamut.Data.Entity;

namespace Alamut.Data.Repository
{
    public interface ISmartRepository<TEntity> : IRepository<TEntity>
        where TEntity : IEntity
    {

    }
}