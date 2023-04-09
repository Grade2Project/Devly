using Devly.Database.Basics.Context;
using Microsoft.EntityFrameworkCore;

namespace Devly.Database.Context
{
    public class DevlyDbContext : EfDbContext
    {
        /*
         * Чтобы добавить таблицу над прописать
         * 
         *      public DbSet<TableName> TableNames { get; set; }
         * 
         */
        
        public DevlyDbContext(DbContextOptions<DevlyDbContext> options) : base(options)
        {
        }
    }
}