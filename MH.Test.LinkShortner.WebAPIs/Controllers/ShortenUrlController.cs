using MH.Test.LinkShortner.WebAPIs.Models;
using MH.Test.LinkShortner.WebAPIs.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MH.Test.LinkShortner.WebAPIs.Controllers
{
    /// <summary>
    /// Main controller class for shortening url
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ShortenUrlController : ControllerBase
    {
        private readonly ILogger<ShortenUrlController> _logger;
        private readonly IMortgageHouseDbRepository _dbRepository;
        private string BaseUrl => $"{Request.Scheme}://{Request.Host.Value}";

        public ShortenUrlController(IMortgageHouseDbRepository dbRepository, ILogger<ShortenUrlController> logger)
        {
            _dbRepository = dbRepository;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> ShortenUrl([FromBody] UrlRequest request)
        {
            _logger.LogInformation($"Shorten Url Called with Original Url: {request.OriginalUrl}");
            var url = await _dbRepository.CreateShortenedUrlAsync(request.OriginalUrl, BaseUrl);
            return Ok(new UrlMapping
            {
                OriginalUrl = request.OriginalUrl,
                ShortenedUrl = url.ShortenedUrl,
                CreatedDate = url.CreatedDate
            });
        }

        [HttpGet("{shortenedUrlPart}")]
        public async Task<IActionResult> RedirectUrl(string shortenedUrlPart)
        {
            _logger.LogInformation($"Redirect Url Called with Shortened Url Part: {shortenedUrlPart}");
            var url = await _dbRepository.GetShortenedUrlAsync(shortenedUrlPart);
            if (url == null || string.IsNullOrWhiteSpace(url.OriginalUrl))
            {
                return NotFound();
            }
            return Redirect(url.OriginalUrl);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUrls()
        {
            _logger.LogInformation($"Get All Urls Called");
            var urls = await _dbRepository.GetAllUrls();
            return Ok(urls);
        }
    }
}