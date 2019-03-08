using Demo.Models;
using System.Data.Entity;

namespace Demo.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Company> Company { get; set; }
        public DbSet<Address> Address { get; set; }
    }
}