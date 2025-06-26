using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        public GoogleController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //Login with Google
        [HttpPost("login")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string idToken)
        {
            if (string.IsNullOrEmpty(idToken))
                return BadRequest("ID token is required.");
            try
            {
                var response = await _httpClient.GetAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={idToken}");
                if (!response.IsSuccessStatusCode)
                    return Unauthorized("Invalid ID token.");

                var json = await response.Content.ReadAsStringAsync();
                var userInfo = JsonObject.Parse(json);

                return Ok(new
                {
                    Email = userInfo?["email"]?.ToString(),
                    Name = userInfo?["name"]?.ToString(),
                    Picture = userInfo?["picture"]?.ToString()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,  new { error = ex.Message, stackTrace = ex.StackTrace });
            }
            
        }
    }
}
