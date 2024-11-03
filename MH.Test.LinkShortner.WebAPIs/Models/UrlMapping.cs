namespace MH.Test.LinkShortner.WebAPIs.Models
{
    /// <summary>
    /// Main entity which holds the Url Mapping.
    /// </summary>
    public class UrlMapping
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortenedUrl { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
