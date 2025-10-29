using Backend.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly string _sheetId; // Google Sheet ID
        private readonly SheetsService _sheetsService; // Google Sheets API service

        public PostsController(IConfiguration config)
        {
            _sheetId = config["GOOGLE_SHEET_ID"];

            // Initialize Google Service Account Credential
            GoogleCredential credential;
            using (var stream = new FileStream("service-account.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(SheetsService.Scope.Spreadsheets); // Grant access to Sheets
            }

            // Initialize SheetsService with credentials
            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "SummerGroupProject"
            });
        }

        // GET: api/posts
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var range = "A:F"; // Columns to fetch
            var request = _sheetsService.Spreadsheets.Values.Get(_sheetId, range);
            var response = await request.ExecuteAsync();

            // Map sheet rows to Post model
            var posts = response.Values?
                .Skip(1) // Skip header row
                .Select((row, index) => new Post
                {
                    RowNumber = index + 2, // Sheet row number
                    Title = row.ElementAtOrDefault(0)?.ToString() ?? "",
                    Content = row.ElementAtOrDefault(1)?.ToString() ?? "",
                    ImageUrl = row.ElementAtOrDefault(2)?.ToString(),
                    Author = row.ElementAtOrDefault(3)?.ToString() ?? "",
                    AuthorImgUrl = row.ElementAtOrDefault(4)?.ToString(),
                    CreatedAt = DateTime.TryParse(row.ElementAtOrDefault(5)?.ToString(), out var d) ? d : DateTime.Now
                })
                .ToList();

            return Ok(posts); // Return all posts as JSON
        }

        // POST: api/posts
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePost([FromForm] PostWithImageDto postDto)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(postDto.Title) ||
                string.IsNullOrWhiteSpace(postDto.Content) ||
                string.IsNullOrWhiteSpace(postDto.Author))
                return BadRequest("Invalid post data.");

            string? imagePath = null;

            // Handle image upload if provided
            if (postDto.Image != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(postDto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await postDto.Image.CopyToAsync(stream);

                imagePath = $"/uploads/{uniqueFileName}";
            }

            // Prepare the row to append
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object>
                    {
                        postDto.Title,
                        postDto.Content,
                        imagePath ?? "",
                        postDto.Author,
                        postDto.AuthorImgUrl ?? "",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                    }
                }
            };

            // Append the new row to the sheet
            var appendRequest = _sheetsService.Spreadsheets.Values.Append(valueRange, _sheetId, "A:F");
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            await appendRequest.ExecuteAsync();

            return Ok(new { message = "Post added to Google Sheet!" });
        }

        // PATCH: api/posts/{rowNumber}
        [HttpPatch("{rowNumber}")]
        public async Task<IActionResult> UpdatePost(int rowNumber, [FromBody] Post updatedPost)
        {
            // Validate post data
            if (updatedPost == null ||
                string.IsNullOrEmpty(updatedPost.Title) ||
                string.IsNullOrEmpty(updatedPost.Content))
                return BadRequest("Invalid post data.");

            // Specify the exact row to update
            var range = $"A{rowNumber}:F{rowNumber}";
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object>
                    {
                        updatedPost.Title,
                        updatedPost.Content,
                        updatedPost.ImageUrl ?? "",
                        updatedPost.Author,
                        updatedPost.AuthorImgUrl ?? "",
                        updatedPost.CreatedAt.ToString("yyyy-MM-dd HH:mm")
                    }
                }
            };

            // Update the row in the sheet
            var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, _sheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            await updateRequest.ExecuteAsync();

            return NoContent();
        }

        // DELETE: api/posts/{rowNumber}
        [HttpDelete("{rowNumber}")]
        public async Task<IActionResult> DeletePost(int rowNumber)
        {
            // Prepare batch request to delete a row
            var batchUpdateRequest = new BatchUpdateSpreadsheetRequest
            {
                Requests = new List<Request>
                {
                    new Request
                    {
                        DeleteDimension = new DeleteDimensionRequest
                        {
                            Range = new DimensionRange
                            {
                                SheetId = 0, // Use 0 if only one sheet exists
                                Dimension = "ROWS",
                                StartIndex = rowNumber - 1, // 0-based index
                                EndIndex = rowNumber
                            }
                        }
                    }
                }
            };

            await _sheetsService.Spreadsheets.BatchUpdate(batchUpdateRequest, _sheetId).ExecuteAsync();

            return NoContent();
        }
    }
}
