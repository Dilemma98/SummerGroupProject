using Backend.Models;
using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound("Post not found.");

            return Ok(post);
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

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(postDto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await postDto.Image.CopyToAsync(stream);

                imagePath = $"/uploads/{uniqueFileName}";
            }

            var post = new Post
            {
                Title = postDto.Title,
                Content = postDto.Content,
                Author = postDto.Author,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = imagePath
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] Post updatedPost)
        {
            if (updatedPost == null ||
                string.IsNullOrEmpty(updatedPost.Title) ||
                string.IsNullOrEmpty(updatedPost.Content) ||
                string.IsNullOrEmpty(updatedPost.Author))
            {
                return BadRequest("Invalid post data.");
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound("Post not found.");

            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;
            post.Author = updatedPost.Author;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound("Post not found.");

            _context.Posts.Remove(post);
            try
    {
        await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        // Logga detaljerat fel (om du har ILogger kan du använda det)
        Console.WriteLine($"Fel vid borttagning av post: {ex.Message}");
        return StatusCode(500, "Ett fel uppstod vid borttagning.");
    }

            return NoContent();
        }
    }
}
