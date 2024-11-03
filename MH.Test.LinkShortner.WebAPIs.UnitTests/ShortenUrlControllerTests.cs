using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MH.Test.LinkShortner.WebAPIs.Controllers;
using MH.Test.LinkShortner.WebAPIs.Models;
using MH.Test.LinkShortner.WebAPIs.Repository;
using Microsoft.AspNetCore.Http;

namespace MH.Test.LinkShortner.WebAPIs.Tests;

[TestFixture]
public class ShortenUrlControllerTests
{
    private Mock<IMortgageHouseDbRepository> _mockRepository;
    private Mock<ILogger<ShortenUrlController>> _mockLogger;
    private ShortenUrlController _controller;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IMortgageHouseDbRepository>();
        _mockLogger = new Mock<ILogger<ShortenUrlController>>();
        _controller = new ShortenUrlController(_mockRepository.Object, _mockLogger.Object);

        // Setup HttpContext to mimic the base URL functionality
        var httpContext = new DefaultHttpContext
                          {
                              Request =
                              {
                                  Scheme = "https",
                                  Host = new HostString("localhost", 5001)
                              }
                          };
        _controller.ControllerContext = new ControllerContext
                                        {
                                            HttpContext = httpContext
                                        };
    }

    [Test]
    public async Task ShortenUrl_ValidRequest()
    {
        // Arrange
        var request = new UrlRequest { OriginalUrl = "https://test.com" };
        var shortenedUrl = new UrlMapping
                           {
                               OriginalUrl = request.OriginalUrl,
                               ShortenedUrl = "https://localhost:5001/api/test123",
                               CreatedDate = DateTime.Now
                           };

        _mockRepository
            .Setup(repo => repo.CreateShortenedUrlAsync(request.OriginalUrl, "https://localhost:5001"))
            .ReturnsAsync(shortenedUrl);

        // Act
        var result = await _controller.ShortenUrl(request);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var urlMapping = okResult.Value as UrlMapping;
        Assert.IsNotNull(urlMapping);
        Assert.AreEqual(request.OriginalUrl, urlMapping.OriginalUrl);
        Assert.AreEqual(shortenedUrl.ShortenedUrl, urlMapping.ShortenedUrl);
    }

    [Test]
    public async Task ShortenUrl_NullUrlRequest()
    {
        // Arrange
        UrlRequest request = new UrlRequest()
                             {
                                 OriginalUrl = string.Empty
                             };

        // Act
        var result = await _controller.ShortenUrl(request);

        // Assert
        Assert.IsInstanceOf<BadRequestResult>(result); // This will fail as no null check is implemented in the method.
    }

    [Test]
    public async Task RedirectUrl_ValidResult()
    {
        // Arrange
        var shortenedUrlPart = "api/test123";
        var originalUrl = "https://test.com";
        var urlMapping = new UrlMapping { OriginalUrl = originalUrl };

        _mockRepository
            .Setup(repo => repo.GetShortenedUrlAsync(shortenedUrlPart))
            .ReturnsAsync(urlMapping);

        // Act
        var result = await _controller.RedirectUrl(shortenedUrlPart);

        // Assert
        Assert.IsInstanceOf<RedirectResult>(result);
        var redirectResult = result as RedirectResult;
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual(originalUrl, redirectResult.Url);
    }

    [Test]
    public async Task RedirectUrl_InvalidShortenedUrlPart_ReturnsNotFoundResult()
    {
        // Arrange
        var shortenedUrlPart = "invalid123";
        _mockRepository
            .Setup(repo => repo.GetShortenedUrlAsync(shortenedUrlPart))
            .ReturnsAsync((UrlMapping)null);

        // Act
        var result = await _controller.RedirectUrl(shortenedUrlPart);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task GetAllUrls_ReturnsAllUrlMappings_Valid()
    {
        // Arrange
        var urlMappings = new List<UrlMapping>
                          {
                              new UrlMapping { OriginalUrl = "https://test1.com", ShortenedUrl = "https://localhost:5001/api/test123" },
                              new UrlMapping { OriginalUrl = "https://test2.com", ShortenedUrl = "https://localhost:5001/def456" }
                          };

        _mockRepository
            .Setup(repo => repo.GetAllUrls())
            .ReturnsAsync(urlMappings);

        // Act
        var result = await _controller.GetAllUrls();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var urls = okResult.Value as List<UrlMapping>;
        Assert.IsNotNull(urls);
        Assert.AreEqual(2, urls.Count);
        Assert.AreEqual("https://test1.com", urls[0].OriginalUrl);
        Assert.AreEqual("https://test2.com", urls[1].OriginalUrl);
    }

    [Test]
    public async Task GetAllUrls_NoUrlMappings_ReturnsEmpty()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.GetAllUrls())
            .ReturnsAsync(new List<UrlMapping> ()); // Empty array for no mappings

        // Act
        var result = await _controller.GetAllUrls();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var urls = okResult.Value as List<UrlMapping>;
        Assert.AreEqual(0, urls.Count);
    }

}