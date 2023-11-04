using Microsoft.AspNetCore.Mvc;
using YoloPersonDetectionAPI.Detectors;
using YoloPersonDetectionAPI.Models;

namespace YoloPersonDetectionAPI.Controllers
{
    [ApiController]
    [Route("api/person-detection")]
    public class PersonDetectionController : ControllerBase
    {

        private readonly ILogger<PersonDetectionController> _logger;

        public PersonDetectionController(ILogger<PersonDetectionController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Post([FromBody] ImageRequest request)
        {
            try
            {
                _logger.LogInformation("Endpoint Called");

                int humanCount = 0;

                if(request != null && !string.IsNullOrEmpty(request.Base64Image))
                {
                    byte[] imageBytes = Convert.FromBase64String(request.Base64Image);
                    PersonDetector detector = new PersonDetector(_logger);
                    humanCount = await detector.GetHumansInImage(imageBytes);
                    _logger.LogInformation($"Person found = {humanCount}");
                    
                }
                else
                    _logger.LogWarning("Bad Request, No image data received");

                return Ok(new { NumberOfHumans = humanCount });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return BadRequest(new { Error = "An error occurred while processing the image." });
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Ready to accept post requests with base64 image strings");
        }
    }
}