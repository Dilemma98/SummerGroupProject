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
            new Post { Id = 1, Title = "First Post", Content = "This is the content of the first post.", Author = "Author1", CreatedAt = DateTime.Now, ImageUrl="" },
            new Post { Id = 2, Title = "Second Post", Content = "This is the content of the second post.", Author = "Author2", CreatedAt = DateTime.Now, ImageUrl="" },
        };

        [HttpGet]
        public IActionResult GetPosts()
        {
            return Ok(posts);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePost([FromForm] PostWithImageDto postDto)
        {
            if (string.IsNullOrWhiteSpace(postDto.Title) ||
                string.IsNullOrWhiteSpace(postDto.Content) ||
                string.IsNullOrWhiteSpace(postDto.Author))
            {
                return BadRequest("Invalid post data.");
            }

            string? imagePath = null;

            if (postDto.Image != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(postDto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await postDto.Image.CopyToAsync(stream);
                }

                imagePath = $"/uploads/{uniqueFileName}";
            }

            var post = new Post
            {
                Id = posts.Count + 1,
                Title = postDto.Title,
                Content = postDto.Content,
                Author = postDto.Author,
                CreatedAt = DateTime.Now,
                ImageUrl = imagePath
            };

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
