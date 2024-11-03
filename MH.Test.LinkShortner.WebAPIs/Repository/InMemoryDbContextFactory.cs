using MH.Test.LinkShortner.WebAPIs.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace MH.Test.LinkShortner.WebAPIs.Repository;

/// <summary>
/// Factory class which generates an in memory database for testing locally.
/// </summary>
public class InMemoryDbContextFactory
{
    private readonly MortgageHouseDbContext _dbContext;

    public InMemoryDbContextFactory()
    {
        // Initialize the in-memory database context
        var options = new DbContextOptionsBuilder<MortgageHouseDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        _dbContext = new MortgageHouseDbContext(options);
    }

    public MortgageHouseDbContext GetDbContext()
    {
        return _dbContext;
    }
}