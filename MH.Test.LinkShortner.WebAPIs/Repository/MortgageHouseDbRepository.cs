using MH.Test.LinkShortner.WebAPIs.BusinessLogic;
using MH.Test.LinkShortner.WebAPIs.DataLayer;
using MH.Test.LinkShortner.WebAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace MH.Test.LinkShortner.WebAPIs.Repository
{
    /// <summary>
    /// DB Repository class which holds the Database operations/logic.
    /// </summary>
    public class MortgageHouseDbRepository : IMortgageHouseDbRepository
    {
        private readonly MortgageHouseDbContext _context;

        public MortgageHouseDbRepository(MortgageHouseDbContext context)
        {
            _context = context;
        }

        public async Task<UrlMapping?> GetShortenedUrlAsync(string shortenedUrlPart)
        {
            return await _context.UrlMappings.SingleOrDefaultAsync(u => u.ShortenedUrl.EndsWith(shortenedUrlPart));
        }

        // Helper method for URL validation
        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public async Task<UrlMapping> CreateShortenedUrlAsync(string originalUrl, string baseUrl)
        {
            if (!IsValidUrl(originalUrl))
            {
                throw new ArgumentException("The provided URL is not valid.");
            }

            var existingUrl = await _context.UrlMappings.SingleOrDefaultAsync(url => url.OriginalUrl == originalUrl);
            if (existingUrl != null)
            {
                return existingUrl;
            }

            var shortenedUrl = baseUrl + "/api/ShortenUrl/" +
                               RandomStringGenerator.GetRandomString(6);
            var url = new UrlMapping
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = shortenedUrl
            };
            _context.UrlMappings.Add(url);
            await _context.SaveChangesAsync();
            return url;
        }

        public async Task<List<UrlMapping>> GetAllUrls()
        {
            return await _context.UrlMappings.ToListAsync();
        }
    }
}
