using Banking_Solution_Test.Models;
using System.Xml.Linq;

namespace Banking_Solution_Test.Services
{
    internal interface IAccountService
    {
        public Account Get(int ID)
        {
            return new Account { Id = 0, Name = "", Balance = 0};
        }
        public List<Account> GetAll()
        {
            return new List<Account>();
        }

        public bool UpdateBalance(int ID, decimal value)
        {
            return true;
        }
    }
}