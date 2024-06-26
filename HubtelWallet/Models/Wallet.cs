using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HubtelWallet.Models;

public class Wallet
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [Required]
    public WalletType Type { get; set; }
    
    [Required]
    [MaxLength(17)]
    public string? AccountNumber { get; set; }
    
    [Required]
    public AccountScheme Scheme { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // defaults the createdAt to the "NOW"

    [MaxLength(10)]
    [Required]
    public string? OwnerPhoneNumber { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WalletType
    {
        MobileMoney = 1 ,
        Card
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AccountScheme
    {
        Visa = 1,
        Mastercard,
        Mtn,
        Vodafone,
        AirtelTigo
    }
}