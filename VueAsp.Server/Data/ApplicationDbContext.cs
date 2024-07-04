using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VueAsp.Server.Models;
namespace VueAsp.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<ArticleData> ArticleData { get; set; }
        public DbSet<FormData> FormData { get; set; }
    }
}
