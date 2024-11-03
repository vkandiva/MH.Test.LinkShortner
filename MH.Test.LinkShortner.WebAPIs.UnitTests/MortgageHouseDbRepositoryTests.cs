using MH.Test.LinkShortner.WebAPIs.DataLayer;
using MH.Test.LinkShortner.WebAPIs.Models;
using MH.Test.LinkShortner.WebAPIs.Repository;
using Microsoft.EntityFrameworkCore;

namespace MH.Test.LinkShortner.WebAPIs.UnitTests;

public class MortgageHouseDbRepositoryTests
{
    private MortgageHouseDbContext _context;
    private MortgageHouseDbRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<MortgageHouseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new MortgageHouseDbContext(options);
        _repository = new MortgageHouseDbRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    #region GetShortenedUrlAsync Tests
    [Test]
    public async Task GetShortenedUrlAsync_ValidShortenedUrlPart_ReturnsUrlMapping()
    {
        // Arrange
        var shortenedUrlPart = "test123";
        var urlMapping = new UrlMapping { OriginalUrl = "https://api/test123.com", ShortenedUrl = $"https://localhost:5001/{shortenedUrlPart}" };

        _context.UrlMappings.Add(urlMapping);
        await _context.SaveChangesAsync();

        var repository = new MortgageHouseDbRepository(_context);

        // Act
        var result = await repository.GetShortenedUrlAsync(shortenedUrlPart);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(urlMapping.OriginalUrl, result?.OriginalUrl);
        Assert.AreEqual(urlMapping.ShortenedUrl, result?.ShortenedUrl);
    }


    [Test]
    public async Task GetShortenedUrlAsync_InvalidShortenedUrlPart_ReturnsNull()
    {
        // Arrange
        var shortenedUrlPart = "invalid123";

        // Act
        var result = await _repository.GetShortenedUrlAsync(shortenedUrlPart);

        // Assert
        Assert.IsNull(result);
    }

    #endregion

    #region CreateShortenedUrlAsync Tests

    [Test]
    public async Task CreateShortenedUrlAsync_ValidOriginalUrl_CreatesAndReturnsUrlMapping()
    {
        // Arrange
        var originalUrl = "https://api/test123.com";
        var baseUrl = "https://localhost:5001";

        // Act
        var result = await _repository.CreateShortenedUrlAsync(originalUrl, baseUrl);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(originalUrl, result.OriginalUrl);
        Assert.IsTrue(result.ShortenedUrl.StartsWith(baseUrl));
    }

    [Test]
    public void CreateShortenedUrlAsync_InvalidOriginalUrl_ThrowsArgumentException()
    {
        // Arrange
        var invalidUrl = "invalid_url";
        var baseUrl = "https://localhost:5001";

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _repository.CreateShortenedUrlAsync(invalidUrl, baseUrl));
    }

    [Test]
    public async Task CreateShortenedUrlAsync_AlreadyExistingOriginalUrl_ReturnsExistingUrlMapping()
    {
        // Arrange
        var originalUrl = "https://api/test123.com";
        var baseUrl = "https://localhost:5001";
        var urlMapping = new UrlMapping { OriginalUrl = originalUrl, ShortenedUrl = $"{baseUrl}/test123" };
        _context.UrlMappings.Add(urlMapping);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.CreateShortenedUrlAsync(originalUrl, baseUrl);

        // Assert
        Assert.AreEqual(urlMapping.ShortenedUrl, result.ShortenedUrl);
    }

    #endregion

    #region GetAllUrls Tests

    [Test]
    public async Task GetAllUrls_ReturnsAllUrlMappings()
    {
        // Arrange
        var urlMappings = new List<UrlMapping>
                          {
                              new UrlMapping { OriginalUrl = "https://api/test1231.com", ShortenedUrl = "https://localhost:5001/test123" },
                              new UrlMapping { OriginalUrl = "https://api/test1232.com", ShortenedUrl = "https://localhost:5001/def456" }
                          };
        _context.UrlMappings.AddRange(urlMappings);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllUrls();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
    }

    [Test]
    public async Task GetAllUrls_NoUrlMappings_ReturnsEmptyList()
    {
        // Act
        var result = await _repository.GetAllUrls();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }

    #endregion
}