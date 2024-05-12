using HubtelWallet.Models;

namespace HubtelWallet.Services;

public interface IWalletService
{
    public Wallet? GetById(int id);
    public IEnumerable<Wallet> GetAll();
    public Wallet Create(Wallet wallet);
    public void DeleteById(int walletId);
    public List<Wallet> GetWalletsByOwner(string ownerPhoneNumber);
    public Wallet GetWalletByAccountNumber(string accountNumber);
}