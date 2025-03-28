using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RetailInventoryManager.Models;
public partial class InventoryDbContext : DbContext
{
    public InventoryDbContext()
    {
    }

    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }


    // This method connects the application to the database - where the connection string is stored in the appsettings.json file
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false).Build();

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("LocalSqlConnection"));
        }
    }

    // This method is used to configure the model that was created in the Product class
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("ProductId").ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.QuantityInStock).HasColumnName("QuantityInStock").IsRequired();
            entity.Property(e => e.Price).HasColumnName("Price").IsRequired().HasColumnType("decimal(18,2)");       // 18 digits, 2 decimal places

        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}