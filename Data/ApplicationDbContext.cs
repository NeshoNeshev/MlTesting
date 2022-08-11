using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace Data
{

    using Microsoft.EntityFrameworkCore;
    using System.Configuration;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Judje> Judjes { get; set; }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

       
    }
}
