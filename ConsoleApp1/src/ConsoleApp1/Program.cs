using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ConsoleApp1
{
    public class Program
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

            using (var context = new NorthwindContext(serviceProvider))
            {
                Console.WriteLine(context.Customers.Include(c => c.Orders).First().CustomerID);
            }
        }
    }

    public class NorthwindContext : DbContext
    {
        public NorthwindContext(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Data Source=.\\SQLEXPRESS;Integrated Security=True;Database=Northwind");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderDetail>()
                .ToTable("Order Details")
                .HasKey(od => new { od.OrderID, od.ProductID });
        }

        public DbSet<Customer> Customers { get; set; }
    }

    public class Customer
    {
        public string CustomerID { get; set; }
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

        [NotMapped]
        public bool IsLondon => City == "London";
    }

    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public int? EmployeeID { get; set; }
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
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
    }
}
