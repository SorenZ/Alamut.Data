﻿using System.Collections.Generic;
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
    public class KeyBasedBlogController : ControllerBase
    {
        private readonly ISmartRepository<Blog, int> _blogRepository;

        public KeyBasedBlogController(ISmartRepository<Blog, int> blogRepository)
        {
            _blogRepository = blogRepository;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<BlogViewModel>> Get()
        {
            var result = await _blogRepository.GetAll<BlogViewModel>();
            return Ok(result);
        }

        //[HttpGet("{id}")]
        //public async Task<BlogDto> Get(int id)
        //{
        //    var result = await _blogRepository.Get<BlogDto>(q => q.Id == id);

        //    return result;
        //}

        [HttpGet("{ids}")]
        public async Task<List<Blog>> Gets([FromQuery] IEnumerable<int> ids)
        {
            var result = await _blogRepository.GetByIds(ids);

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