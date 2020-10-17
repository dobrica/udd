using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq;
using udd.Database;

namespace udd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var dbContext = new ScientificCenterDbContext())
            {
                //Ensure database is created
                dbContext.Database.EnsureCreated();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}