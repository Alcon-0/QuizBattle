using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImagesController : ControllerBase
    {
        private readonly MongoImageService _mongoImageService;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(
            MongoImageService mongoImageService,
            ILogger<ImagesController> logger)
        {
            _mongoImageService = mongoImageService;
            _logger = logger;
        }

        [HttpGet("{imageId}")]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetImage(string imageId)
        {
            try
            {
                if (!ObjectId.TryParse(imageId, out var objectId))
                    return BadRequest("Invalid image ID format");

                var (content, contentType) = await _mongoImageService.GetImageWithContentTypeAsync(objectId);
                return File(content, contentType);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving image {ImageId}", imageId);
                return StatusCode(500, "Error retrieving image");
            }
        }
    }
}
