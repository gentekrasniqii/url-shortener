using Microsoft.EntityFrameworkCore;
using url_shortener_app.Data;

public class LinkShortener
{
    private readonly AppDbContext _dbContext;
    private readonly Random _random = new();

    public LinkShortener(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GenerateUniqueCode()
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        const int codeLength = 6;
        var codeChars = new char[codeLength];

        for (var i = 0; i < codeLength; i++)
        {
            codeChars[i] = alphabet[_random.Next(alphabet.Length)];
        }

        var code = new string(codeChars);
        return code;
    }
}
