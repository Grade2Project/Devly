using Devly.Database.Basics.Context;
using Devly.Database.Models;
using Devly.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Devly.Database.Context
{
    public class DevlyDbContext : EfDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UsersPassword> UsersPasswords { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<UsersFavoriteLanguage> UsersFavoriteLanguages { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<CompanyPassword> CompaniesPasswords { get; set; }

        public DevlyDbContext(DbContextOptions<DevlyDbContext> options) : base(options)
        {
        }
    }
}