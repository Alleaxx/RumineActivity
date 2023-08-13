using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivityAPI.Models;
using RumineActivityAPI.Services;
using Microsoft.EntityFrameworkCore;

using RumineActivity.Core;
using RumineActivity.Core.Models;

namespace RumineActivityAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopicsController : ControllerBase
    {
        private readonly RsContext RsContext;
        public TopicsController(RsContext rsContext)
        {
            RsContext = rsContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> Get(bool? isChat)
        {
            if (isChat.HasValue)
            {
                return await RsContext.Topics.Where(t => t.IsChat == isChat).ToListAsync();
            }
            return await RsContext.Topics.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Topic>> Post(Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Topic found = await RsContext.Topics.FirstOrDefaultAsync(p => p.Id == topic.Id);
            if (found == null)
            {
                RsContext.Topics.Add(topic);
                await RsContext.SaveChangesAsync();
            }
            return topic;
        }
        [HttpPut]
        public async Task<ActionResult<Topic>> Put(Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!RsContext.Topics.Any(p => p.Id == topic.Id))
            {
                return NotFound();
            }

            RsContext.Update(topic);
            await RsContext.SaveChangesAsync();
            return topic;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Topic>> Delete(int id)
        {
            var found = await RsContext.Topics.FirstOrDefaultAsync(p => p.Id == id);
            if (found == null)
            {
                return NotFound();
            }

            RsContext.Topics.Remove(found);
            await RsContext.SaveChangesAsync();
            return found;
        }
    }
}
