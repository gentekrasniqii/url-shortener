namespace url_shortener_app.Models
{
    public class Link
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
