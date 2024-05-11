using HubtelWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace HubtelWallet.Data;

public class WalletContext: DbContext
{
    public WalletContext(DbContextOptions<WalletContext> options)
    : base(options)
    {
        
    }

    public DbSet<Wallet> Wallets => Set<Wallet>();
}