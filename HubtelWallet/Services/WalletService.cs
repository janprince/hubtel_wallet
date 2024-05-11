using HubtelWallet.Data;
using HubtelWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace HubtelWallet.Services;

public class WalletService
{
    public readonly WalletContext _context;

    public WalletService(WalletContext context)
    {
        _context = context;
    }

    public Wallet? GetById(int id)
    {
        return _context.Wallets
            .AsNoTracking()
            .SingleOrDefault(w => w.Id == id);
    }

    public IEnumerable<Wallet> GetAll()
    {
        return _context.Wallets
            .AsNoTracking()
            .ToList();
    }

    public Wallet Create(Wallet wallet)
    {
        if (wallet is { AccountNumber.Length: > 5, Type: Wallet.AccountType.Card })
        {
            wallet.AccountNumber = wallet.AccountNumber.Substring(0, 6);
        }
        
        _context.Wallets.Add(wallet);
        _context.SaveChanges();

        return wallet;
    }

    public void DeleteById(int walletId)
    {
        var walletToDelete = _context.Wallets.Find(walletId);

        if (walletToDelete is null)
        {
            throw new InvalidOperationException();
        }
        
        throw new InvalidOperationException("Wallet does not exist");
        
    }

    public List<Wallet> GetWalletsByOwner(string ownerPhoneNumber)
    {
        return _context.Wallets
            .Where(w => w.OwnerPhoneNumber == ownerPhoneNumber)
            .ToList();
    }

    public Wallet GetWalletByAccountNumber(string accountNumber)
    {
        return _context.Wallets
            .First(w => w.AccountNumber == accountNumber);
    }
}