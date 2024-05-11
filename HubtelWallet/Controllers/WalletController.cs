using HubtelWallet.Models;
using HubtelWallet.Services;
using Microsoft.AspNetCore.Mvc;

namespace HubtelWallet.Controllers;

[ApiController]
[Route("[controller]")]
public class WalletController : ControllerBase
{
    private WalletService _service;

    public WalletController(WalletService service)
    {
        _service = service;
    }

    [HttpGet]
    public IEnumerable<Wallet> GetAll()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Wallet> GetById(int id)
    {
        var wallet = _service.GetById(id);

        if (wallet is null)
        {
            return NotFound();
        }

        return wallet;
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var walletToDelete = _service.GetById(id);

        if (walletToDelete is not null)
        {
            _service.DeleteById(id);
            return NoContent();
        }
        else
        {
            return NotFound("Wallet Not Found");
        }
        
    }

    [HttpPost]
    public ActionResult Create(Wallet wallet)
    {
        
        // check if maximum wallet limit has been exceeded 
        if (wallet.OwnerPhoneNumber != null)
        {
            var walletsByOwner = _service.GetWalletsByOwner(wallet.OwnerPhoneNumber);
            if (walletsByOwner.Count >= 5)
            {
                return BadRequest("Maximum wallet number exceeded");
            }
        }
        
        // check for duplicate wallets
        var allWallets = _service.GetAll();
        var accountWallet = allWallets.FirstOrDefault(w => w.AccountNumber == wallet.AccountNumber);
        if (accountWallet is not null)
        {
            return Conflict("A wallet with the same account number already exists");
        }

        _service.Create(wallet);
        return CreatedAtAction(nameof(GetById), new { id = wallet.Id }, wallet);
    }
}