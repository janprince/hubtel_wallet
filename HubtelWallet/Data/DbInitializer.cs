using HubtelWallet.Models;

namespace HubtelWallet.Data;

public static class DbInitializer
{
    public static void Initialize(WalletContext context)
    {
        if (context.Wallets.Any())
        {
            return; // DB has been seeded
        }

        var wallets = new Wallet[]
        {
            new Wallet
            {
                Name = "John's Mobile Money Wallet",
                Type = Wallet.WalletType.MobileMoney,
                AccountNumber = "0551234567",
                Scheme = Wallet.AccountScheme.Mtn,
                CreatedAt = DateTime.UtcNow,
                OwnerPhoneNumber = "0551234567"
            },
            new Wallet
            {
                Name = "Jane's Visa Card Wallet",
                Type = Wallet.WalletType.Card,
                AccountNumber = "123456",
                Scheme = Wallet.AccountScheme.Visa,
                CreatedAt = DateTime.UtcNow,
                OwnerPhoneNumber = "0209876543"
            }
        };

        context.Wallets.AddRange(wallets);
        context.SaveChanges();
        
    }
}