using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using url_shortener_app.Data;
using url_shortener_app.Models;

public class HomeController : Controller
{
    private readonly LinkShortener _urlShorteningService;
    private readonly AppDbContext _dbContext;

    public HomeController(LinkShortener urlShorteningService, AppDbContext dbContext)
    {
        _urlShorteningService = urlShorteningService;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var links = _dbContext.Links.ToList();
        return View(links);
    }


    [HttpPost]
    public async Task<IActionResult> ShortenUrl(string originalUrl, int expirationTime)
    {
        if (!originalUrl.StartsWith("http://") && !originalUrl.StartsWith("https://"))
        {
            originalUrl = "http://" + originalUrl;
        }

        DateTime expirationDateTime = DateTime.UtcNow.AddMinutes(expirationTime);

        var shortCode = await _urlShorteningService.GenerateUniqueCode();

        var shortenedUrl = new Link
        {
            Id = Guid.NewGuid(),
            OriginalUrl = originalUrl,
            ShortenedUrl = $"{Request.Scheme}://{Request.Host}/{shortCode}",
            ExpirationTime = expirationDateTime
        };

        _dbContext.Links.Add(shortenedUrl);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction("Index");
    }



    [HttpGet("{code}")]
    public async Task<IActionResult> RedirectToOriginal(string code)
    {
        var link = await _dbContext.Links.FirstOrDefaultAsync(s => s.ShortenedUrl.EndsWith($"/{code}"));
        if (link == null)
        {
            return NotFound("Linku ka skaduar / nuk ekziston");
        }
        else if (link.ExpirationTime < DateTime.UtcNow)
        {
            return StatusCode(StatusCodes.Status410Gone, "Linku skadoi.");
        }
        return Redirect(link.OriginalUrl);
    }


    [HttpPost]
    public async Task<IActionResult> DeleteUrl(Guid id)
    {
        var link = await _dbContext.Links.FindAsync(id);
        _dbContext.Links.Remove(link);

        await _dbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }


}
