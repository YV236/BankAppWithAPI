using BankAppWithAPI.Data;
using BankAppWithAPI.Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace BankAppWithAPI.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static async Task<User> FindUser(this ClaimsPrincipal userToFind, DataContext context)
        {
            var id = userToFind.GetNameIdentifier();

            return await userToFind.FindEntityAsync<User>(
                context,
                user => user.Id.ToString() == id,
                query => query.Include(u => u.Card!).Include(u => u.AccountCards!).ThenInclude(ac => ac.Account)
            )!;
        }

        public static async Task<BankAccount> FindUserActiveAccount(this ClaimsPrincipal userToFind, DataContext context)
        {
            var id = userToFind.GetNameIdentifier();

             var user = await userToFind.FindEntityAsync<User>(
                context,
                card => card.Id.ToString() == id,
                query => query.Include(c => c.AccountCards).ThenInclude(ac => ac.Account)
                )!;

            return user!.AccountCards.First(ac => ac.Account!.IsActive).Account!;
        }

        public static async Task<BankAccount> FindCardActiveAccount(this ClaimsPrincipal cardToFind, DataContext context)
        {
            var id = cardToFind.GetNameIdentifier();

            var card = await cardToFind.FindEntityAsync<Card>(
                context,
                card => card.Id.ToString() == id,
                query => query.Include(c => c.AccountCards).ThenInclude(ac => ac.Account)
                )!;

            return card!.AccountCards.First(ac => ac.Account!.IsActive).Account!;
        }

        private static string GetNameIdentifier(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }

        private static async Task<T?> FindEntityAsync<T>(
            this ClaimsPrincipal claimsPrincipal,
            DataContext context,
            Expression<Func<T, bool>> filterByClaims,
            Func<IQueryable<T>, IQueryable<T>>? include = null
            ) where T : class
        {
            var query = context.Set<T>().AsQueryable();

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(filterByClaims);
        }

    }
}
