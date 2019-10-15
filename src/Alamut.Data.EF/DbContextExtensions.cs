using System.Threading;
using System.Threading.Tasks;

using Alamut.Abstractions.Structure;

using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.EF
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// convert the result of <code>DbContext.SaveChangesAsync()</code> to the <see cref="Result"/> object
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<Result> SaveChangeAndReturnResult(this DbContext dbContext,
            CancellationToken cancellationToken)
        {
            var updatedItems = await dbContext.SaveChangesAsync(cancellationToken);
            return updatedItems > 0
                ? Result.Okay($"{updatedItems} updated on the database")
                : Result.Error("no change were made to the database");
        }

        public static Result SaveChangeAndReturnResult(this DbContext dbContext)
        {
            var updatedItems = dbContext.SaveChanges();
            return updatedItems > 0
                ? Result.Okay($"{updatedItems} updated on the database")
                : Result.Error("no change were made to the database");
        }
            
            
    }
}
