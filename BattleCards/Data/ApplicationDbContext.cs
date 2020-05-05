namespace BattleCards.Data
{
    using BattleCards.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    { 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserCard>()
                .HasKey(pm => new { pm.UserId, pm.CardId });

            modelBuilder.Entity<UserCard>()
                .HasOne(u => u.User)
                .WithMany(uc => uc.UserCards)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<UserCard>()
                .HasOne(u => u.Card)
                .WithMany(u => u.UserCards)
                .HasForeignKey(u => u.CardId);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<UserCard> UserCards { get; set; }
    }
}
