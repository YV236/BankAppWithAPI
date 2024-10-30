using BankAppWithAPI.Data;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Extensions
{
    public static class CardExtension
    {
        public static async Task<BankAccount> FindActiveAccount (this string cardNumber, DataContext _context)
        {
            var card = await _context.Cards.Include(c => c.AccountCards).FirstOrDefaultAsync(c => c.CardNumber == cardNumber);

            return card!.AccountCards.FirstOrDefault(u => u.Account!.IsActive == true)!.Account!;
        }
    }
}
