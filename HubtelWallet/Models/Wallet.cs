using System.ComponentModel.DataAnnotations;
namespace HubtelWallet.Models;

public class Wallet
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [Required]
    public AccountType Type { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string? AccountNumber { get; set; }
    
    [Required]
    public AccountScheme Scheme { get; set; }
    public DateTime CreatedAt { get; set; }
    
    [MaxLength(10)]
    [Required]
    public string? OwnerPhoneNumber { get; set; }

    public enum AccountType
    {
        MobileMoney,
        Card
    }

    public enum AccountScheme
    {
        Visa,
        Mastercard,
        Mtn,
        Vodafone,
        AirtelTigo
    }
    
}