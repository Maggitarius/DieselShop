using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace apidiesel.Models;

public partial class dieselContext : DbContext
{
    public dieselContext()
    {
    }

    public dieselContext(DbContextOptions<dieselContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }

    public virtual DbSet<OperationDetail> OperationDetails { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("workstation id=dieseldb.mssql.somee.com;packet size=4096;user id=savinegor_SQLLogin_1;pwd=zfuuu1xdz7;data source=dieseldb.mssql.somee.com;persist security info=False;initial catalog=dieseldb;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD797B53E7C79");

            entity.ToTable("Cart");

            entity.HasIndex(e => new { e.CustomerId, e.ProductId }, "UQ_Customer_Product").IsUnique();

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.AddedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2BB1C874DC");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(true);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8602C34BA");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(true);
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .IsUnicode(true);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(200)
                .IsUnicode(true)
                .HasDefaultValue("");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(true);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF15454BCBD");
            entity.HasIndex(e => e.Login, "UQ__Employee__5E55825B00B267A7").IsUnique();
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .IsUnicode(true);
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(true);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(200)
                .IsUnicode(true);
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .IsUnicode(true);
        });

        modelBuilder.Entity<Operation>(entity =>
        {
            entity.HasKey(e => e.OperationId).HasName("PK__Operatio__A4F5FC645A1B8040");
            entity.Property(e => e.OperationId).HasColumnName("OperationID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.OperationType)
                .HasMaxLength(50)
                .IsUnicode(true);
            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");
        });

        modelBuilder.Entity<OperationDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__Operatio__135C314D067E1D78");
            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.OperationId).HasColumnName("OperationID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFC49732FE");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(true)
                .HasDefaultValue("Новый");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30C5B61B08F");
            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED6CB118EC");
            entity.HasIndex(e => e.Sku, "UQ__Products__CA1ECF0DF0D97BE0").IsUnique();
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .IsUnicode(true);
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .IsUnicode(true);
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .IsUnicode(true)
                .HasColumnName("SKU");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("PK__Stock__2C83A9E2E88FE929");
            entity.ToTable("Stock");
            entity.Property(e => e.StockId).HasColumnName("StockID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666948A32F517");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.Contacts)
                .HasMaxLength(250)
                .IsUnicode(true);
            entity.Property(e => e.SupplierName)
                .HasMaxLength(200)
                .IsUnicode(true);
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFD9C14F0563");
            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(true);
            entity.Property(e => e.WarehouseName)
                .HasMaxLength(150)
                .IsUnicode(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
