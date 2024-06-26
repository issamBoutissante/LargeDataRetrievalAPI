using LargeDataRetrievalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LargeDataRetrievalAPI.Data
{
    public class LargeDataContext : DbContext
    {
        public LargeDataContext(DbContextOptions<LargeDataContext> options) : base(options) { }

        public DbSet<LargeDataRetrievalAPI.Models.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LargeDataRetrievalAPI.Models.Task>()
                .HasOne(t => t.AssignedToUser)
                .WithMany(u=>u.Tasks)
                .HasForeignKey(t => t.AssignedToUserId);
        }
    }

}
