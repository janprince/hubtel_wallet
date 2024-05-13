using System;
using HubtelWallet.Controllers;
using HubtelWallet.Models;
using HubtelWallet.Data;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HubtelWallet.Dto;
using Moq;
using Microsoft.AspNetCore.Mvc;
using HubtelWallet.Services;
using Xunit.Abstractions; // Add this using directive


namespace HubtelWallet.Tests.Controllers;

public class WalletControllerTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public WalletControllerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GetWalletsByOwner_WithValidPhoneNumber_ReturnsListOfWallets()
    {
        // Arrange
        var phoneNumber = "1234567890";
        var wallets = new List<Wallet>
        {
            new Wallet
            {
                Id = 1, OwnerPhoneNumber = phoneNumber, Type = Wallet.WalletType.MobileMoney,
                AccountNumber = "23232333", Name = "First Wallet", Scheme = Wallet.AccountScheme.Vodafone,
                CreatedAt = DateTime.Now
            },
            new Wallet
            {
                Id = 2, OwnerPhoneNumber = phoneNumber, Type = Wallet.WalletType.Card, AccountNumber = "232333",
                Name = "Second Wallet", Scheme = Wallet.AccountScheme.Vodafone, CreatedAt = DateTime.Now
            },
        };
        var serviceMock = new Mock<IWalletService>();
        serviceMock.Setup(repo => repo.GetWalletsByOwner(phoneNumber)).Returns(wallets);
        var mapperMock = new Mock<IMapper>();
        var controller = new WalletController(serviceMock.Object, mapperMock.Object);

        // Act
        var result = controller.GetWalletsByOwner(phoneNumber);

        // Assert
        var actionResult = Assert.IsType<List<Wallet>>(result);
        Assert.Equal(wallets, wallets);
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
        var mapperMock = new Mock<IMapper>();
        serviceMock.Setup(repo => repo.GetById(wallet.Id)).Returns(wallet);
        var controller = new WalletController(serviceMock.Object, mapperMock.Object);

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
        var mapperMock = new Mock<IMapper>();
        serviceMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Wallet)null);
        var controller = new WalletController(serviceMock.Object, mapperMock.Object);

        // Act
        var result = controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Create_WithValidWallet_ReturnsCreatedAtAction()
    {
        // Arrange
        var walletPost = new WalletPostDto
        {
            OwnerPhoneNumber = "1234567890",
            Type = Wallet.WalletType.MobileMoney,
            AccountNumber = "1234567890123456",
            Scheme = Wallet.AccountScheme.Mtn
        };
        var walletResponse = new Wallet
        {
            Id = 1,
            OwnerPhoneNumber = "1234567890",
            Type = Wallet.WalletType.MobileMoney,
            AccountNumber = "1234567890123456",
            Scheme = Wallet.AccountScheme.Mtn
        };
        var serviceMock = new Mock<IWalletService>();
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<Wallet>(It.IsAny<WalletPostDto>())).Returns(walletResponse);
        
        serviceMock.Setup(repo => repo.GetWalletsByOwner(walletPost.OwnerPhoneNumber)).Returns(new List<Wallet>());
        serviceMock.Setup(repo => repo.GetAll()).Returns(new List<Wallet>());
        serviceMock.Setup(repo => repo.Create(walletResponse)).Returns(walletResponse);
        var controller = new WalletController(serviceMock.Object, mapperMock.Object);

        // Act
        var result = controller.Create(walletPost);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetById", actionResult.ActionName);
        Assert.Equal(1, actionResult.RouteValues["id"]); // checking if the id is returned
    }

    [Fact]
    public void Create_WithExceededMaxWallets_ReturnsBadRequest()
    {
        // Arrange
        var wallet = new Wallet
        {
            OwnerPhoneNumber = "1234567890"
        };
        var walletPost = new WalletPostDto
        {
            OwnerPhoneNumber = "1234567890"
        };
        var wallets = new List<Wallet>
        {
            new Wallet(), new Wallet(), new Wallet(), new Wallet(), new Wallet() // 5 wallets
        };
        var serviceMock = new Mock<IWalletService>();
        var mapperMock = new Mock<IMapper>();
        
        // Setup IMapper Mock
        mapperMock.Setup(m => m.Map<Wallet>(It.IsAny<WalletPostDto>())).Returns(wallet);
        
        serviceMock.Setup(repo => repo.GetWalletsByOwner(wallet.OwnerPhoneNumber)).Returns(wallets);
        var controller = new WalletController(serviceMock.Object, mapperMock.Object);

        // Act
        var result = controller.Create(walletPost);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Maximum wallet number exceeded", actionResult.Value);
    }

    [Fact]
    public void Create_WithDuplicateAccountNumber_ReturnsConflict()
    {
        // Arrange
        var walletPost = new WalletPostDto { AccountNumber = "1234567890123456" };
        var wallet = new Wallet { AccountNumber = "1234567890123456" };
        
        var wallets = new List<Wallet>
        {
            new Wallet { Type = Wallet.WalletType.Card, AccountNumber = "123456" }
        };
        var serviceMock = new Mock<IWalletService>();
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<Wallet>(It.IsAny<WalletPostDto>())).Returns(wallet);
        serviceMock.Setup(repo => repo.GetAll()).Returns(wallets);
        var controller = new WalletController(serviceMock.Object, mapperMock.Object);
    
        // Act
        var result = controller.Create(walletPost);
    
        // Assert
        var actionResult = Assert.IsType<ConflictObjectResult>(result);
        _testOutputHelper.WriteLine(actionResult.Value.ToString());
        Assert.Equal("A wallet with the same account number already exists", actionResult.Value);
    }

    [Fact]
    public void Create_WithInvalidAccountScheme_ReturnsBadRequest()
    {
        // Arrange
        var walletPost = new WalletPostDto { Type = Wallet.WalletType.Card, Scheme = Wallet.AccountScheme.Mtn };
        var wallet = new Wallet { Type = Wallet.WalletType.Card, Scheme = Wallet.AccountScheme.Mtn };
        
        var serviceMock = new Mock<IWalletService>();
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<Wallet>(It.IsAny<WalletPostDto>())).Returns(wallet);
        
        var controller = new WalletController(serviceMock.Object, mapperMock.Object);

        // Act
        var result = controller.Create(walletPost);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid account scheme for the selected type", actionResult.Value);
    }

    [Fact]
    public void Delete_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var walletId = 1;
        var walletToDelete = new Wallet { Id = walletId };
        var serviceMock = new Mock<IWalletService>();
        var mapperMock = new Mock<IMapper>();
        serviceMock.Setup(repo => repo.GetById(walletId)).Returns(walletToDelete);
        var controller = new WalletController(serviceMock.Object, mapperMock.Object);

        // Act
        var result = controller.Delete(walletId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        serviceMock.Verify(repo => repo.DeleteById(walletId), Times.Once);
    }

    [Fact]
    public void Delete_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var walletId = 1;
        var serviceMock = new Mock<IWalletService>();
        var mapperMock = new Mock<IMapper>();
        serviceMock.Setup(repo => repo.GetById(walletId)).Returns((Wallet)null);
        var controller = new WalletController(serviceMock.Object, mapperMock.Object);

        // Act
        var result = controller.Delete(walletId);

        // Assert
        var actionResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Wallet Not Found", actionResult.Value);
        serviceMock.Verify(repo => repo.DeleteById(walletId), Times.Never);
    }
}