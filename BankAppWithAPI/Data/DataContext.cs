using BankAppWithAPI.Models;
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
        public DbSet<BankAccountCard> BankAccountCards { get; set; }
        public DbSet<User> Users { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);

        //     modelBuilder.Entity<BankAccountCard>()
        //         .HasKey(bac => new { bac.Id });

        //     modelBuilder.Entity<BankAccountCard>()
        // .HasOne(bac => bac.Account)
        // .WithMany(a => a.AccountCards)
        // .HasForeignKey(bac => bac.AccountId)
        // .OnDelete(DeleteBehavior.Restrict)
        // .IsRequired(false);

        //     modelBuilder.Entity<BankAccountCard>()
        //.HasOne(bac => bac.Card)
        //.WithMany(c => c.AccountCards)
        //.HasForeignKey(bac => bac.CardId)
        //.OnDelete(DeleteBehavior.Restrict)
        //.IsRequired(false);

        //     modelBuilder.Entity<User>(entity =>
        //     {
        //         entity.HasIndex(e => e.Email).IsUnique();
        //         entity.Property(e => e.UserName).HasMaxLength(50).IsRequired();
        //         entity.Property(e => e.Email).HasMaxLength(319).IsRequired();
        //         entity.Property(e => e.UserFirstName).HasMaxLength(100);
        //         entity.Property(e => e.UserLastName).HasMaxLength(100);
        //         entity.Property(e => e.Address).HasMaxLength(200);
        //         entity.Property(e => e.PhoneNumber).HasMaxLength(15);
        //     });

        //     modelBuilder.Entity<BankAccount>(entity =>
        //     {
        //         entity.Property(e => e.AccountNumber).HasMaxLength(20).IsRequired();
        //         entity.Property(e => e.AccountPriority).HasMaxLength(20);
        //         entity.Property(e => e.AccountName).HasMaxLength(100);
        //         entity.Property(e => e.DateOfCreation).IsRequired();
        //     });

        //     modelBuilder.Entity<Card>(entity =>
        //     {
        //         entity.Property(e => e.CardNumber).HasMaxLength(16).IsRequired();
        //         entity.Property(e => e.CVVHash).IsRequired();
        //         entity.Property(e => e.CVVSalt).IsRequired();
        //         entity.Property(e => e.ExpiryDate).IsRequired();
        //     });

        //     modelBuilder.HasDefaultSchema("UserIdentity");
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфігурація для BankAccountCard
            modelBuilder.Entity<BankAccountCard>()
                .HasKey(bac => bac.Id);

            modelBuilder.Entity<BankAccountCard>()
                .HasOne(bac => bac.Account)
                .WithMany(a => a.AccountCards)
                .HasForeignKey(bac => bac.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<BankAccountCard>()
                .HasOne(bac => bac.Card)
                .WithMany(c => c.AccountCards)
                .HasForeignKey(bac => bac.CardId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // Конфігурація для User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.UserName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(319).IsRequired();
                entity.Property(e => e.UserFirstName).HasMaxLength(100);
                entity.Property(e => e.UserLastName).HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            });

            // Конфігурація для BankAccount
            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.Property(e => e.AccountNumber).HasMaxLength(20).IsRequired();
                entity.Property(e => e.AccountPriority).HasMaxLength(20);
                entity.Property(e => e.AccountName).HasMaxLength(100);
                entity.Property(e => e.DateOfCreation).IsRequired();
            });

            // Конфігурація для Card
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
