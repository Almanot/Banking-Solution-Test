using Banking_Solution_Test.Models;
using Banking_Solution_Test.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking_Solution_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankController : ControllerBase
    {
        public BankController()
        {
        }

        [HttpGet]
        public ActionResult<List<Account>> GetAll() => AccountService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Account> Get(int id)
        {
            var acc = AccountService.Get(id);
            if (acc == null) return NotFound();
            return acc;
        }

        [HttpPost]
        public IActionResult Create(Account acc)
        {
            AccountService.Add(acc);
            return CreatedAtAction(nameof(Get), new { id = acc.Id }, acc);
        }

        [HttpPut("deposit")]
        public IActionResult Deposit(int id, decimal amount)
        {
            
            var existingAccount = AccountService.Get(id);
            if (existingAccount is null)
                return NotFound();

            amount = Math.Abs(amount);

            if (AccountService.UpdateBalance(id, amount)) return NoContent();
            else return BadRequest();
        }

        [HttpPut("withdraw")]
        public IActionResult Withdraw(int id, decimal amount)
        {
            var existingAccount = AccountService.Get(id);
            if (existingAccount is null)
                return NotFound();

            // pre changings before send to balance update.
            amount = amount > 0? -amount : amount;

            // Send to balance update already changed negative value
            if (AccountService.UpdateBalance(id, amount)) return NoContent();
            else return BadRequest();
        }

        [HttpPut("transfer")]
        public IActionResult Transfer(int senderID, int recipientID, decimal amount)
        {
            var senderAccount = AccountService.Get(senderID);
            if (senderAccount is null)
                return NotFound("Sender not found");

            var recipientAccount = AccountService.Get(recipientID);
            if (recipientAccount is null)
                return NotFound("Recipient not found");

            amount = Math.Abs(amount);

            if (AccountService.UpdateBalance(senderID, -amount) && AccountService.UpdateBalance(recipientID, amount)) return NoContent();

            else return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var acc = AccountService.Get(id);

            if (acc is null)
                return NotFound();

            AccountService.Delete(id);
            return NoContent();
        }
    }
}
