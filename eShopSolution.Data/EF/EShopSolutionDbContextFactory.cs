using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace eShopSolution.Data.EF
{
    public class EShopSolutionDbContextFactory : IDesignTimeDbContextFactory<EShopDataContext>
    {
        IConfigurationRoot configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        public EShopSolutionDbContextFactory()
        {
            
        }

        public EShopDataContext CreateDbContext(string[] args)
        {
            var connectionString = configurationRoot.GetConnectionString("eShopSolutionDb");

            var optionsBuilder = new DbContextOptionsBuilder<EShopDataContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new EShopDataContext(optionsBuilder.Options);
        }
    }
}
