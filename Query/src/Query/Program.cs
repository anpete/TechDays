using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace Update
{
    internal class Program
    {
        public static void Main()
        {
            #region ServiceProvider

            var serviceProvider
                = new ServiceCollection()
                    .AddEntityFramework()
                    .AddSqlServer()
                    .GetInfrastructure()
                    .BuildServiceProvider();

            serviceProvider.GetService<ILoggerFactory>().AddConsole( /*(_, __) => true*/);

            #endregion

            using (var context = new NorthwindContext(serviceProvider))
            {
                
            }
        }
    }

    public class NorthwindContext : DbContext
    {
        public NorthwindContext(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(@"Server=.\SQLEXPRESS;Database=Northwind;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderDetail>()
                .ToTable("Order Details")
                .HasKey(od => new {od.OrderId, od.ProductId});
        }
    }

    public class Customer
    {
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public bool HasSwissBankAccount(PostalCodeService postalCodeService) 
            => postalCodeService.IsSwitzerland(PostalCode);

        public override string ToString() 
            => $"{CustomerId} - {CompanyName} - {City} - {Country}";
    }

    public class PostalCodeService
    {
        public bool IsSwitzerland(string postalCode) 
            => postalCode == "3012";
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

        public Customer Customer { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
    }
}