using Microsoft.EntityFrameworkCore;
using System.Reflection;
using udd.Model;

namespace udd.Database
{
    public class ScientificCenterDbContext : DbContext
    {
        public DbSet<ScientificPaper> ScientificPapers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=ScientificCenterDatabase.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<ScientificPaper>().ToTable("ScientificPaper", "test");
            modelBuilder.Entity<ScientificPaper>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Title).IsUnique();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}