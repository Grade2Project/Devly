using Devly.Database.Basics.Context;
using Devly.Database.Models;
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
        public DbSet<UsersFavoriteVacancy> UsersFavoriteVacancies { get; set; }
        public DbSet<CompaniesFavoriteUser> CompaniesFavoriteUsers { get; set; }

        public DevlyDbContext(DbContextOptions<DevlyDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersFavoriteLanguage>()
                .HasKey(nameof(UsersFavoriteLanguage.UserLogin),
                    nameof(UsersFavoriteLanguage.ProgrammingLanguageId));

            modelBuilder.Entity<UsersFavoriteVacancy>()
                .HasKey(nameof(UsersFavoriteVacancy.UserLogin),
                    nameof(UsersFavoriteVacancy.VacancyId));

            modelBuilder.Entity<CompaniesFavoriteUser>()
                .HasKey(nameof(CompaniesFavoriteUser.CompanyId),
                    nameof(CompaniesFavoriteUser.UserLogin));

        }
    }
}