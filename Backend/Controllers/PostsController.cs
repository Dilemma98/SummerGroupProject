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
    }
}
