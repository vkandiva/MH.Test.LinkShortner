using MH.Test.LinkShortner.WebAPIs.Models;

namespace MH.Test.LinkShortner.WebAPIs.Repository;

/// <summary>
/// Interface for the DB repository.
/// </summary>
public interface IMortgageHouseDbRepository
{
    Task<UrlMapping> GetShortenedUrlAsync(string shortenedUrl);
    Task<UrlMapping> CreateShortenedUrlAsync(string requestOriginalUrl, string baseUrl);
    Task<List<UrlMapping>> GetAllUrls();
}