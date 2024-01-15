using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PriceTracker.Infrastructure.Context;

namespace Services.UnitTests.Data;

public static class TestAppDbContextData
{
    private static readonly DbContextOptions<AppDbContext> ContextOptionsMock = Substitute.For<DbContextOptions<AppDbContext>>();

    public static AppDbContext GetMockDbContext()
    {
        var contextMock = Substitute.For<AppDbContext>(ContextOptionsMock);
        
        return contextMock;
    }
}