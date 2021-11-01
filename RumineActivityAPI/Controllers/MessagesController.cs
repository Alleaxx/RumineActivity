using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivityAPI.Models;
using RumineActivityAPI.Services;
using Microsoft.EntityFrameworkCore;

using RumineActivity.Core;
namespace RumineActivityAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly RsContext RsContext;
        public MessagesController(RsContext rsContext)
        {
            RsContext = rsContext;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> Get(int? topicId, DateTime? from, DateTime? to)
        {
            var messages = RsContext.Posts.Take(5000);

            if (topicId.HasValue)
            {
                messages = messages.Where(msg => msg.TopicId == topicId);
            }
            if (from.HasValue)
            {
                messages = messages.Where(msg => msg.Date >= from.Value);
            }
            if (to.HasValue)
            {
                messages = messages.Where(msg => msg.Date <= to.Value);
            }

            return await messages.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Post>> Post(Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Post found = await RsContext.Posts.Include(p => p.Topic).FirstOrDefaultAsync(p => p.Id == post.Id);
            if(found == null)
            {
                EnsureTopicCreated(post.TopicId);
                RsContext.Posts.Add(post);
                await RsContext.SaveChangesAsync();
            }
            return post;
        }
        [HttpPut]
        public async Task<ActionResult<Post>> Put(Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!RsContext.Posts.Any(p => p.Id == post.Id))
            {
                return NotFound();
            }

            EnsureTopicCreated(post.TopicId);
            RsContext.Update(post);
            await RsContext.SaveChangesAsync();
            return post;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Post>> Delete(int id)
        {
            var found = await RsContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if(found == null)
            {
                return NotFound();
            }

            RsContext.Posts.Remove(found);
            await RsContext.SaveChangesAsync();
            return found;
        }


        private async void EnsureTopicCreated(int id)
        {
            Topic foundTopic = await RsContext.Topics.FirstOrDefaultAsync(t => t.Id == id);
            if (foundTopic == null)
            {
                Topic addTopic = new Topic() { Name = "Новая тема", Id = id };
                RsContext.Topics.Add(addTopic);
            }
        }
    }
}
