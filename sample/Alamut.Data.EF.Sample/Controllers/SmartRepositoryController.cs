using System.Threading.Tasks;
using Alamut.Data.EF.Test.Database;
using Alamut.Data.EF.Test.Models;
using Alamut.Data.NoSql;
using Alamut.Data.Paging;
using Alamut.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Alamut.Data.EF.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmartRepositoryController : ControllerBase
    {
        private readonly ISmartRepository<Blog> _blogRepository;

        public SmartRepositoryController(ISmartRepository<Blog> blogRepository)
        {
            _blogRepository = blogRepository;
        }
        
        // GET api/values
        [HttpGet]
        public async Task<IPaginated<BlogViewModel>> Get()
        {
            return await _blogRepository.GetPaginated<BlogViewModel>(new DynamicPaginatedCriteria
            {
                CurrentPage = 1,
                PageSize = 10,
                
                Sorts = "Id desc",
                // FilterClause = "Id == @0",
                // FilterParameters = new object[] {1}
            });
        }
    }
}