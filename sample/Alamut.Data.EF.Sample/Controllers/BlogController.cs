using System.Threading;
using System.Threading.Tasks;
using Alamut.Abstractions.Structure;
using Alamut.Data.EF.Test.Database;
using Alamut.Data.EF.Test.Models;
using Alamut.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Alamut.Data.EF.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ISmartRepository<Blog> _blogRepository;

        public BlogController(ISmartRepository<Blog> blogRepository)
        {
            _blogRepository = blogRepository;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<BlogDto>> Get()
        {
            var result = await _blogRepository.GetAll<BlogDto>();
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDto>> Get(int id)
        {
            return await _blogRepository.GetById<BlogDto>(id);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<Result>> Post([FromBody] BlogDto value)
        {
            _blogRepository.Add(value);
            
            var result = await _blogRepository.CommitAsync(CancellationToken.None);

            return result ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Result>> Put(int id, [FromBody] BlogDto value)
        {
            value.Id = id;
            _blogRepository.Update<BlogDto>(value);

            var result = await _blogRepository.CommitAsync(CancellationToken.None);

            return result ? Ok(result) : StatusCode(result.StatusCode, result);

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            _blogRepository.DeleteById(id);
            var result = await _blogRepository.CommitAsync(CancellationToken.None);

            return result ? Ok(result) : StatusCode(result.StatusCode, result);
        }
    }
}
