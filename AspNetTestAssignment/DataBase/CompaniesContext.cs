using AspNetTestAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetTestAssignment.DataBase
{
    public class CompaniesContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "Companies");
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<History> CompanyHistories { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}
