using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Update
{
    internal class Program
    {
        public static void Main()
        {
            var serviceProvider
                = new ServiceCollection()
                    .AddEntityFramework()
                    .AddSqlServer()
                    .GetInfrastructure()
                    .BuildServiceProvider();

            serviceProvider.GetService<ILoggerFactory>().AddConsole();

            using (var context = new BlogContext(serviceProvider))
            {
                for (var i = 0; i < 20; i++)
                {
                    context.Add(
                        new Blog
                        {
                            Name = $"Blog {i}",
                            Url = $"http://sample.com/blog{i}"
                        });
                }

                context.SaveChanges();
            }
        }
    }

    public class BlogContext : DbContext
    {
        public BlogContext(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public DbSet<Blog> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(@"Server=.\SQLEXPRESS;Database=Update;Trusted_Connection=True;")
                .MaxBatchSize(1);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }

    public class Blog
    {
        public Guid BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}