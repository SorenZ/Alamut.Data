using Alamut.Data.Entity;
using Alamut.Data.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.EF
{
    public class SmartRepository<TEntity> : SmartRepository<TEntity, int>, 
        ISmartRepository<TEntity> where TEntity : class, IEntity, new()
    {
        /// <inheritdoc />
        public SmartRepository(DbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
