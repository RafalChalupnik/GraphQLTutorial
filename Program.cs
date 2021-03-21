using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLTutorial.DataAccess;
using GraphQLTutorial.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GraphQLTutorial
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }

        private static void SeedDb(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var author = context.Authors.Add(
                new Author
                {
                    Name = "First Author",
                }
            );

            context.SaveChanges();

            context.Books.AddRange(
                new Book
                {
                    Name = "First Book",
                    Published = true,
                    AuthorId = author.Entity.Id,
                    Genre = "Mystery"
                },
                new Book
                {
                    Name = "Second Book",
                    Published = true,
                    AuthorId = author.Entity.Id,
                    Genre = "Crime"
                }
            );
        }
    }
}
