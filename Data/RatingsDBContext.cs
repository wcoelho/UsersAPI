using Microsoft.EntityFrameworkCore;
using UsersAPI.Models;
 
namespace UsersAPI.Data
{
    public class RatingsDBContext : DbContext
    {
        public RatingsDBContext(DbContextOptions<RatingsDBContext> options)
            : base(options)
        {
        }
         public DbSet<Rating> Ratings { get; set; }
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("Rating_seq", schema: "dbo")
            .StartsAt(3)
            .IncrementsBy(1);

            modelBuilder.Entity<Rating>()
            .Property(r => r.RatingId)
            .HasDefaultValueSql("NEXT VALUE FOR dbo.Rating_seq");

        }

    }
}