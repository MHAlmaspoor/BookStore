// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration;


// namespace BookStore.IdentityService.Infrastructure.Persistence;

// public sealed class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
// {
//     public IdentityDbContext CreateDbContext(string[] args)
//     {
//         var configuration = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("../BookStore.IdentityService.Api/appsettings.json")
//             .Build();

//         var options = new DbContextOptionsBuilder<IdentityDbContext>()
//             .UseNpgsql(configuration.GetConnectionString("IdentityDatabase"))
//             .Options;

//         return new IdentityDbContext(options);
//     }
// }
