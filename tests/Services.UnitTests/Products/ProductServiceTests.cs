using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using PriceTracker.Domain.Entities;
using PriceTracker.Infrastructure.Context;
using PriceTracker.Services.Product;

namespace Services.UnitTests.Products;

public class ProductServiceTests
{
    private static readonly long FakeId = 1;
    private static readonly Product FakeProduct = new("fakeMarketPlace", "Test", "Test") { Id = FakeId };
    private readonly IDbContextFactory<AppDbContext> _contextFactoryMock;
    private readonly AppDbContext _contextMock;
    private readonly ProductService _productService;
    

    public ProductServiceTests()
    {
        _contextFactoryMock = Substitute.For<IDbContextFactory<AppDbContext>>();
        _contextMock = Substitute.For<AppDbContext>();
        
        _productService = new ProductService(_contextFactoryMock);
    }

    [Fact]
    public async Task RemoveProductById_ShouldNotRemoveProduct_WhenProductNotFound()
    {
        // Arrange
        _contextFactoryMock.CreateDbContextAsync(Arg.Any<CancellationToken>())
            .Returns(_contextMock);
        var mock = Array.Empty<Product>().BuildMock().BuildMockDbSet();
        _contextMock.Products.Returns(mock);
        
        // Act
        await _productService.RemoveProductById(FakeId);
    
        // Assert
    
        _contextMock.Products
            .DidNotReceive()
            .Remove(Arg.Any<Product>());
    }
    
    [Fact]
    public async Task RemoveProductById_ShouldRemoveProduct_WhenProductExists()
    {
        // Arrange
        _contextFactoryMock.CreateDbContextAsync(Arg.Any<CancellationToken>())
            .Returns(_contextMock);
        var mock = GetFakeProducts().BuildMock().BuildMockDbSet();
        _contextMock.Products.Returns(mock);
        
        // Act
        await _productService.RemoveProductById(FakeId);
    
        // Assert
    
        _contextMock.Products
            .Received(1)
            .Remove(Arg.Any<Product>());

        await _contextMock
            .Received(1)
            .SaveChangesAsync();
    }

    private List<Product> GetFakeProducts()
    {
        return new List<Product>
        {
            FakeProduct
        };
    }
    
}