using BlazorAppWeb.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWeb.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //
        }

        public DbSet<User> Users { get; set; }
    }
}
