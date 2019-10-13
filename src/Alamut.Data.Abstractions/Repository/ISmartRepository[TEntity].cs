using Alamut.Data.Abstractions.Entity;

namespace Alamut.Data.Abstractions.Repository
{
    public interface ISmartRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : IEntity
    {

    }
}