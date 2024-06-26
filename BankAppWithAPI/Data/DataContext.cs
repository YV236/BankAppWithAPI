
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BankAppWithAPI.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankAccountCard>()
                .HasKey(bac => new { bac.AccountId, bac.CardId });

            modelBuilder.Entity<BankAccountCard>()
                .HasOne(bac => bac.Account)
                .WithMany(a => a.AccountCards)
                .HasForeignKey(bac => bac.AccountId);

            modelBuilder.Entity<BankAccountCard>()
                .HasOne(bac => bac.Card)
                .WithMany(c => c.AccountCards)
                .HasForeignKey(bac => bac.CardId);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.UserName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            });

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.Property(e => e.AccountNumber).HasMaxLength(20).IsRequired();
                entity.Property(e => e.AccountPriority).HasMaxLength(20);
                entity.Property(e => e.AccountName).HasMaxLength(100);
                entity.Property(e => e.DateOfCreation).IsRequired();
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.Property(e => e.CardNumber).HasMaxLength(16).IsRequired();
                entity.Property(e => e.CVVHash).IsRequired();
                entity.Property(e => e.CVVSalt).IsRequired();
                entity.Property(e => e.ExpiryDate).IsRequired();
            });

        }
    }
}
