using HubtelWallet.Models;
using HubtelWallet.Services;
using Microsoft.AspNetCore.Mvc;

namespace HubtelWallet.Controllers;

[ApiController]
[Route("[controller]")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _service;

    public WalletController(IWalletService service)
    {
        _service = service;
    }

    [HttpGet("userWallets/{phoneNumber}")]
    public IEnumerable<Wallet> GetWalletsByOwner(string phoneNumber)
    {
        return _service.GetWalletsByOwner(phoneNumber);
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
        
        var accountWallet = wallet.Type == Wallet.WalletType.MobileMoney 
            ? allWallets.FirstOrDefault(w => w.AccountNumber == wallet.AccountNumber) 
            : allWallets.FirstOrDefault(w => w.AccountNumber == wallet.AccountNumber.Substring(0, 6));
        
        if (accountWallet is not null)
        {
            return Conflict("A wallet with the same account number already exists");
        }
        
        // check if account scheme is of the right type
        if (wallet.Type == Wallet.WalletType.Card)
        {
            if (wallet.Scheme != Wallet.AccountScheme.Mastercard && wallet.Scheme != Wallet.AccountScheme.Visa)
            {
                return BadRequest("Invalid account scheme for the selected type");
            }
        }
        else
        {
            if (wallet.Scheme != Wallet.AccountScheme.Mtn && wallet.Scheme != Wallet.AccountScheme.AirtelTigo &&
                wallet.Scheme != Wallet.AccountScheme.Vodafone)
            {
                return BadRequest("Invalid account scheme for the selected type");
            }
        }

        _service.Create(wallet);
        return CreatedAtAction(nameof(GetById), new { id = wallet.Id }, wallet);
    }
}