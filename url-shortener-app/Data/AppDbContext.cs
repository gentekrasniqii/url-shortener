using Microsoft.EntityFrameworkCore;
using url_shortener_app.Models;

namespace url_shortener_app.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
        }
        public DbSet<Link> Links { get; set; }
    }
}
