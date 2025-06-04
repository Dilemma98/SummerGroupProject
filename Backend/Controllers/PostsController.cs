using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private static List<Post> posts = new List<Post>
        {
            new Post { Id = 1, Title = "First Post", Content = "This is the content of the first post.", Author = "Author1", CreatedAt = DateTime.Now },
            new Post { Id = 2, Title = "Second Post", Content = "This is the content of the second post.", Author = "Author2", CreatedAt = DateTime.Now }
        };

        [HttpGet]
        public IActionResult GetPosts()
        {
            return Ok(posts);
        }

        [HttpPost]
        public IActionResult CreatePost([FromBody] Post post)
        {
            if (post == null || string.IsNullOrEmpty(post.Title) || string.IsNullOrEmpty(post.Content) || string.IsNullOrEmpty(post.Author))
            {
                return BadRequest("Invalid post data.");
            }

            // Simple ID generation
            post.Id = posts.Count + 1;
            post.CreatedAt = DateTime.Now;
            posts.Add(post);
            return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdatePost(int id, [FromBody] Post updatedPost)
        {
            var post = posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            if (updatedPost == null || string.IsNullOrEmpty(updatedPost.Title) || string.IsNullOrEmpty(updatedPost.Content) || string.IsNullOrEmpty(updatedPost.Author))
            {
                return BadRequest("Invalid post data.");
            }

            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;
            post.Author = updatedPost.Author;
            // Return no content to indicate success!
            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(int id)
        {
            var post = posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound("Post not found.");
            }
            return Ok(post);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            var post = posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            posts.Remove(post);
            return NoContent();
        }
    }
}
