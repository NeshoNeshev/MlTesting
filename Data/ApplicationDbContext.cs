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

        public DbSet<Case> Cases { get; set; }

        public DbSet<Court> Courts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(ConfigurationData.ConnectionString);
                
            }
        }
       
    }
}
