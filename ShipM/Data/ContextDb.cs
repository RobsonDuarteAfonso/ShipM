using Microsoft.EntityFrameworkCore;
using ShipM.Models;

namespace ShipM.Data
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions<ContextDb> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Ship> Ships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);
        }
    }
}
