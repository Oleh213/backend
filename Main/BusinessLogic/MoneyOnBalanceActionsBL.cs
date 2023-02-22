using System;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;

namespace WebShop.Main.BusinessLogic
{
	public class MoneyOnBalanceActionsBL : IMoneyOnBalanceActionsBL
    {
        private ShopContext _context;

        public MoneyOnBalanceActionsBL(ShopContext context)
        {
            _context = context;
        }

        public async Task<Cards> GetCartd(string cardNumber)
        {
            return await _context.cards.FirstOrDefaultAsync(number => number.CardNumber == cardNumber);
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async  Task<string> TopUpBalance(User user, Cards card, int requestedAmount)
        {
            user.AccountBalance += requestedAmount;

            card.Balance -= requestedAmount;

            await _context.SaveChangesAsync();

            return "Ok";
        }

    }
}

