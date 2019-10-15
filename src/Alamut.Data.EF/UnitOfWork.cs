using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Alamut.Abstractions.Structure;
using Alamut.Data.Sql;

using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Result Commit() => _dbContext.SaveChangeAndReturnResult();

        public async Task<Result> CommitAsync(CancellationToken cancellationToken = default) => 
            await _dbContext.SaveChangeAndReturnResult(cancellationToken);

        public void RejectChanges()
        {
            var changedEntries = _dbContext.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (entry.State) 
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
