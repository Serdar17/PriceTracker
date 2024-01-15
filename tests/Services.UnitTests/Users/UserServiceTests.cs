using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using PriceTracker.Domain.Entities;
using PriceTracker.Domain.Repositories;
using PriceTracker.Infrastructure.Context;
using PriceTracker.Services.User;

namespace Services.UnitTests.Users;

public class UserServiceTests : IDisposable, IAsyncDisposable
{
    private static readonly long FakeId = 1;
    private static readonly User FakeUser = new() { Id = FakeId, FirstName = "fakeUser"};
    private static readonly Product FakeProduct = new("fakeMarketPlace", "Test", "Test") { Id = FakeId };

    private readonly IDbContextFactory<AppDbContext> _contextFactoryMock;
    private readonly AppDbContext _contextMock;
    private readonly IUserService _userServiceMock;

    public UserServiceTests()
    {
        _contextFactoryMock = Substitute.For<IDbContextFactory<AppDbContext>>();
        _contextMock = Substitute.For<AppDbContext>();

        _userServiceMock = new UserService(_contextFactoryMock);
    }

    [Fact]
    public async Task CreateUser_ShouldCreateUser_IfUserDontExists()
    {
        // Arrange 
        _contextFactoryMock.CreateDbContextAsync()
            .Returns(_contextMock);
        var mock = GetFakeUsers().BuildMock().BuildMockDbSet();
        _contextMock.Users.Returns(mock);
        var user = new User { Id = 2, FirstName = "Test" };
        
        // Act
        await _userServiceMock.CreateUserAsync(user);
        
        // Assert
        _contextMock.Users.Received(1).Add(Arg.Is<User>(x => x.Id == user.Id));
        _contextMock.Users.Count().Should().Be(1);
        await _contextMock.Received(1).SaveChangesAsync();
    }
    
    [Fact]
    public async Task CreateUser_ShouldDoNothing_IfUserExists()
    {
        // Arrange 
        _contextFactoryMock.CreateDbContextAsync()
            .Returns(_contextMock);
        var mock = GetFakeUsers().BuildMock().BuildMockDbSet();
        _contextMock.Users.Returns(mock);
        
        // Act
        await _userServiceMock.CreateUserAsync(FakeUser);
        
        // Assert
        _contextMock.Users.DidNotReceive().Add(Arg.Any<User>());
        _contextMock.Users.Count().Should().Be(1);
        await _contextMock.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task AddProductToUser_ShouldAddProduct_IfUserExists()
    {
        // Arrange
        _contextFactoryMock.CreateDbContextAsync()
            .Returns(_contextMock);
        var mock = GetFakeUsers().BuildMock().BuildMockDbSet();
        _contextMock.Users.Returns(mock);
        
        // Act
        await _userServiceMock.AddProductToUserAsync(FakeId, FakeProduct);

        // Assert
        FakeUser.Products.Count.Should().Be(1);
        _contextMock.Users.Received(1).Update(Arg.Is<User>(x => x.Id == FakeUser.Id));
        await _contextMock.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetProductByUserId_ShouldReturnProduct_IfUserExists()
    {
        // Arrange
        _contextFactoryMock.CreateDbContextAsync()
            .Returns(_contextMock);
        var mock = GetFakeUserWithProducts().BuildMock().BuildMockDbSet();
        _contextMock.Users.Returns(mock);
        
        // Act
        var products = (await _userServiceMock.GetProductsByUserIdAsync(FakeId)).ToList();
        var product = products[0];
        
        // Assert
        products.Count.Should().Be(1);
        product.Id.Should().Be(FakeProduct.Id);
        product.Title.Should().Be(FakeProduct.Title);
    }
    
    [Fact]
    public async Task GetProductByUserId_ShouldReturnEmptyList_IfUserDontHaveProducts()
    {
        // Arrange
        _contextFactoryMock.CreateDbContextAsync()
            .Returns(_contextMock);
        var mock = GetFakeUsers().BuildMock().BuildMockDbSet();
        _contextMock.Users.Returns(mock);
        
        // Act
        var products = (await _userServiceMock.GetProductsByUserIdAsync(FakeId)).ToList();
        
        // Assert
        products.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUsers_ShouldReturnUser()
    {
        // Arrange
        _contextFactoryMock.CreateDbContextAsync()
            .Returns(_contextMock);
        var mock = GetFakeUserWithProducts().BuildMock().BuildMockDbSet();
        _contextMock.Users.Returns(mock);
        
        // Act
        var users = (await _userServiceMock.GetUsersAsync()).ToList();
        var user = users[0];
        
        // Assert
        users.Count.Should().Be(1);
        user.Should().BeSameAs(user);
        user.Products.Count.Should().Be(1);
        user.Products.First().Should().BeSameAs(FakeProduct);
    }

    private IEnumerable<User> GetFakeUsers()
    {
        return new List<User>
        {
            FakeUser
        };
    }

    private IEnumerable<User> GetFakeUserWithProducts()
    {
        FakeUser.Products = new List<Product> { FakeProduct };
        return GetFakeUsers();
    }

    public void Dispose()
    {
        FakeUser.Products = new List<Product>();
    }

    public async ValueTask DisposeAsync()
    {
        FakeUser.Products = new List<Product>();
    }
}