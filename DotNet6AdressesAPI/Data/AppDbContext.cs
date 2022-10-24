using DotNet6AdressesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet6AdressesAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Address> Addresses { get; set; }
    }
}
