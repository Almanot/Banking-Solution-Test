using Banking_Solution_Test.Models;

namespace Banking_Solution_Test.Services
{
    public class AccountService : IAccountService
    {
        static List<Account> Accounts { get; }
        static int nextId = 3;
        static AccountService()
        {
            Accounts = new List<Account>
            {
                new Account { Id = 1, Name = "Andrew", Balance = 100},
                new Account { Id = 2, Name = "Antony", Balance = 150}
            };
        }

        public static List<Account> GetAll() => Accounts;

        public static Account? Get(int id) => Accounts.FirstOrDefault(p => p.Id == id);

        public static void Add(Account acc)
        {
            acc.Id = nextId++;
            Accounts.Add(acc);
        }

        public static void Delete(int id)
        {
            var Account = Get(id);
            if (Account is null)
                return;

            Accounts.Remove(Account);
        }

        public static void Update(Account acc)
        {
            var index = Accounts.FindIndex(p => p.Id == acc.Id);
            if (index == -1)
                return;

            Accounts[index] = acc;
        }

        public static bool UpdateBalance(int id, decimal value)
        {
            // Check account validity
            var account = Get(id);
            if (account is null) return false;

            // Save values before changing them
            decimal oldBalance = account.Balance;
            account.Balance += value;

            // An attempt to change values.
            if (account.Balance == oldBalance + value)
                return true;
            else
            {
                // If attempt to change value fails, then attempt to return value back and exit with false.
                account.Balance = oldBalance;
                return false;
            }
        }
    }
}
