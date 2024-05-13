using System;
using HubtelWallet.Controllers;
using HubtelWallet.Models;
using HubtelWallet.Data;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.AspNetCore.Mvc;
using HubtelWallet.Services; // Add this using directive


namespace HubtelWallet.Tests.Controllers;

public class WalletControllerTests
{
    [Fact]
    public void GetAll_ReturnsListOfWallets()
    {
        // Arrange
        var serviceMock = new Mock<IWalletService>();
        var wallets = new List<Wallet>
        {
            new Wallet { Id = 1, OwnerPhoneNumber = "1234567890", Type = Wallet.WalletType.MobileMoney, AccountNumber = "23232333", Name = "First Wallet", Scheme = Wallet.AccountScheme.Vodafone, CreatedAt = DateTime.Now},
            new Wallet {Id = 2, OwnerPhoneNumber = "0987654321", Type = Wallet.WalletType.Card, AccountNumber = "232333", Name = "First Wallet", Scheme = Wallet.AccountScheme.Vodafone, CreatedAt = DateTime.Now},
        };
        serviceMock.Setup(repo => repo.GetAll()).Returns(wallets);
        var controller = new WalletController(serviceMock.Object);

        // Act
        var result = controller.GetAll();

        // Assert
        // var actionResult = Assert.IsType<ActionResult<IEnumerable<Wallet>>>(result);
        // var model = Assert.IsAssignableFrom<IEnumerable<Wallet>>(actionResult.Value);
        // Assert.Equal(2, model.Count());
        
        Assert.Equal(wallets, result);
    }
    
    [Fact]
    public void GetById_WithValidId_ReturnsWallet()
    {
        // Arrange
        var wallet = new Wallet
        {
            Id = 2, OwnerPhoneNumber = "0987654321", Type = Wallet.WalletType.Card, AccountNumber = "232333",
            Name = "First Wallet", Scheme = Wallet.AccountScheme.Vodafone, CreatedAt = DateTime.Now
        };
        var serviceMock = new Mock<IWalletService>();
        serviceMock.Setup(repo => repo.GetById(wallet.Id)).Returns(wallet);
        var controller = new WalletController(serviceMock.Object);

        // Act
        var result = controller.GetById(wallet.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Wallet>>(result);
        var model = Assert.IsType<Wallet>(actionResult.Value);
        Assert.Equal(wallet, model);
    }
    
    [Fact]
    public void GetById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var serviceMock = new Mock<IWalletService>();
        serviceMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Wallet)null);
        var controller = new WalletController(serviceMock.Object);

        // Act
        var result = controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}