using Devly.Database.Basics.Context;
using Devly.Database.Models;
using Devly.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Devly.Database.Context
{
    public class DevlyDbContext : EfDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DevlyDbContext(DbContextOptions<DevlyDbContext> options) : base(options)
        {
        }
    }
}