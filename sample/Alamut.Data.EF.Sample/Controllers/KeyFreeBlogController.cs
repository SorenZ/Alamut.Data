﻿using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Alamut.Abstractions.Structure;
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
    public class KeyFreeBlogController : ControllerBase
    {
        private readonly ISmartRepository<Blog> _blogRepository;

        public KeyFreeBlogController(ISmartRepository<Blog> blogRepository)
        {
            _blogRepository = blogRepository;
        }

        // GET api/values
        [HttpGet]
        public async Task<IPaginated<Blog>> Get()
        {
            //var result = await _blogRepository.GetAll<BlogDto>();
            //return Ok(result);

            return await _blogRepository.GetPaginated(new DynamicPaginatedCriteria
            {
                Sorts = "Id desc",
                FilterClause = "Id == @0",
                FilterParameters = new object[] {1}
            });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<BlogViewModel> Get(int id)
        {
            var result = await _blogRepository.Get<BlogViewModel>(q => q.Id == id);

            return result;
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<Result>> Post([FromBody] BlogViewModel value)
        {
            _blogRepository.Add(value);
            
            var result = await _blogRepository.CommitAsync(CancellationToken.None);

            return result ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Result>> Put(int id, [FromBody] BlogViewModel value)
        {
            await _blogRepository.UpdateById(id, value);

            var result = await _blogRepository.CommitAsync(CancellationToken.None);

            return result ? Ok(result) : StatusCode(result.StatusCode, result);

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            await _blogRepository.DeleteById(id);
            var result = await _blogRepository.CommitAsync(CancellationToken.None);

            return result ? Ok(result) : StatusCode(result.StatusCode, result);
        }
    }
}
