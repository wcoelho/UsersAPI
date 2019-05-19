using Microsoft.EntityFrameworkCore;
using UsersAPI.Models;
 
namespace UsersAPI.Data
{
    public class UsersDBContext : DbContext
    {
        public UsersDBContext(DbContextOptions options)
            : base(options)
        {
        }
         public DbSet<User> Users { get; set; }
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("User_seq", schema: "dbo")
            .StartsAt(3)
            .IncrementsBy(1);

            modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .HasDefaultValueSql("NEXT VALUE FOR dbo.User_seq");

        }

    }
}