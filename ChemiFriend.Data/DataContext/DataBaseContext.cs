using ChemiFriend.Entity;
using ChemiFriend.ENTITY;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ChemiFriend.Data.DataContext
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base("name=connectionstring")
        { }
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Usermaster> Usermasters { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<LicenceImages> LicenceImages { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductSubCategory> ProductSubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Scheme> Schemes { get; set; }
        public DbSet<SchemeType> SchemeTypes { get; set; }
        public DbSet<DealType> DealTypes { get; set; }
        public DbSet<FormType> FormTypes { get; set; }
        public DbSet<PackType> PackTypes { get; set; }
        public DbSet<GSTApplicable> GSTApplicables { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<DealApplicableFor> DealApplicableFors { get; set; }
        public DbSet<ProductCode> productCodes { get; set; }


    }
}
