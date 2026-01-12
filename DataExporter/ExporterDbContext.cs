using DataExporter.Model;
using Microsoft.EntityFrameworkCore;

namespace DataExporter
{
    public class ExporterDbContext : DbContext
    {
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Note> Notes { get; set; }

        public ExporterDbContext(DbContextOptions<ExporterDbContext> options) : base(options)
        { 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("ExporterDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Policy>().HasData(
                new Policy() { Id = 1, PolicyNumber = "HSCX1001", Premium = 200, StartDate = new DateTime(2024, 4, 1) },
                new Policy() { Id = 2, PolicyNumber = "HSCX1002", Premium = 153, StartDate = new DateTime(2024, 4, 5) },
                new Policy() { Id = 3, PolicyNumber = "HSCX1003", Premium = 220, StartDate = new DateTime(2024, 3, 10) },
                new Policy() { Id = 4, PolicyNumber = "HSCX1004", Premium = 200, StartDate = new DateTime(2024, 5, 1) },
                new Policy() { Id = 5, PolicyNumber = "HSCX1005", Premium = 100, StartDate = new DateTime(2024, 4, 1) });

            modelBuilder.Entity<Note>().HasData(
                new Note() { Id = 1, Text = "This is a test note", PolicyId = 1 },
                new Note() { Id = 2, Text = "This is a second note", PolicyId = 1 },
                new Note() { Id = 3, Text = "This is a test note", PolicyId = 2 },
                new Note() { Id = 4, Text = "This is a second note", PolicyId = 2 },
                new Note() { Id = 5, Text = "This is a test note", PolicyId = 3 },
                new Note() { Id = 6, Text = "This is a second note", PolicyId = 3 },
                new Note() { Id = 7, Text = "This is a test note", PolicyId = 4 },
                new Note() { Id = 8, Text = "This is a second note", PolicyId = 4 },
                new Note() { Id = 9, Text = "This is a test note", PolicyId = 5 },
                new Note() { Id = 10, Text = "This is a second note", PolicyId = 5 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
