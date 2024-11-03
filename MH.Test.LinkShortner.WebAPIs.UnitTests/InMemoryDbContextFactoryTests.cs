using Microsoft.EntityFrameworkCore;
using MH.Test.LinkShortner.WebAPIs.DataLayer;
using MH.Test.LinkShortner.WebAPIs.Repository;
using MH.Test.LinkShortner.WebAPIs.Models;

namespace MH.Test.LinkShortner.WebAPIs.Tests;

[TestFixture]
public class InMemoryDbContextFactoryTests
{
    private InMemoryDbContextFactory _dbContextFactory;

    [SetUp]
    public void Setup()
    {
        _dbContextFactory = new InMemoryDbContextFactory();
    }

    [Test]
    public void GetDbContext_ReturnsDbContextInstance()
    {
        // Act
        var dbContext = _dbContextFactory.GetDbContext();

        // Assert
        Assert.IsNotNull(dbContext);
        Assert.IsInstanceOf<MortgageHouseDbContext>(dbContext);
    }

    [Test]
    public async Task GetDbContext_CanAddAndRetrieveData()
    {
        // Arrange
        var dbContext = _dbContextFactory.GetDbContext();

        // Add test data
        dbContext.UrlMappings.Add(new UrlMapping { OriginalUrl = "https://api/test123.com", ShortenedUrl = "abc123" });
        await dbContext.SaveChangesAsync();

        // Act
        var urlMapping = await dbContext.UrlMappings.FirstOrDefaultAsync(u => u.ShortenedUrl == "abc123");

        // Assert
        Assert.IsNotNull(urlMapping);
        Assert.AreEqual("https://api/test123.com", urlMapping.OriginalUrl);
        Assert.AreEqual("abc123", urlMapping.ShortenedUrl);
    }
}