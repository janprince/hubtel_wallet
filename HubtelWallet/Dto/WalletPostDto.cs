using System.ComponentModel.DataAnnotations;
using HubtelWallet.Models;

namespace HubtelWallet.Dto;

public class WalletPostDto
{
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [Required]
    public Wallet.WalletType Type { get; set; }
    
    [Required]
    [MaxLength(17)]
    public string? AccountNumber { get; set; }
    
    [Required]
    public Wallet.AccountScheme Scheme { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // defaults the createdAt to the "NOW"

    [MaxLength(10)]
    [Required]
    public string? OwnerPhoneNumber { get; set; }
}