using System;
using System.Collections.Generic;
using Microsoft.Data.Entity;

namespace Metadata
{
    public class Program
    {
        public static void Main()
        {
            using (var context = new NorthwindContext())
            {
                foreach (var entityType in context.Model.GetEntityTypes())
                {
                    Console.Write(entityType.ClrType.Name);
                    Console.Write(" => ");
                    Console.WriteLine(entityType.SqlServer().TableName);
                }
            }
        }
    }

    public class NorthwindContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) 
            => options.UseSqlServer("Data Source=.\\SQLEXPRESS;Integrated Security=True;Database=Northwind");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId });

            #region Model convention

//            foreach (var entity in modelBuilder.Model.GetEntityTypes())
//            {
//                modelBuilder
//                    .Entity(entity.Name)
//                    .ToTable("tbl_" + entity.ClrType.Name.ToLower());
//            }

            #endregion
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