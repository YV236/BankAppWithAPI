
namespace BankAppWithAPI.Data
{
    public class DataContext : DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Card> Cards => Set<Card>();
        public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
        public DbSet<User> Users => Set<User>();
    }
}
