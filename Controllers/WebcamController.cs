using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace WebcamApp.Controllers
{
    [ApiController]
    [Route("api/webcam")]
    public class WebcamController : ControllerBase
    {
        [HttpPost("upload")]
        public IActionResult UploadImage([FromBody] WebcamImageModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Image))
            {
                return BadRequest("Invalid image data.");
            }

            // Extract the base64 data
            var base64Data = Regex.Match(model.Image, @"data:image/(?<type>.+?);base64,(?<data>.+)").Groups["data"].Value;
            var imageBytes = Convert.FromBase64String(base64Data);

            // Save the image (e.g., to a file or database)
            var filePath = Path.Combine("wwwroot/images", $"{Guid.NewGuid()}.png");
            System.IO.File.WriteAllBytes(filePath, imageBytes);

            return Ok(new { message = "Image uploaded successfully.", filePath });
        }
    }

    public class WebcamImageModel
    {
        public string Image { get; set; }
    }
}
