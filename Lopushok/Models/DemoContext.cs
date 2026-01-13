using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lopushok.Models;

public partial class DemoContext : DbContext
{
    public DemoContext()
    {
    }

    public DemoContext(DbContextOptions<DemoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agent> Agents { get; set; }

    public virtual DbSet<Agentpriorityhistory> Agentpriorityhistories { get; set; }

    public virtual DbSet<Agenttype> Agenttypes { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Materialcounthistory> Materialcounthistories { get; set; }

    public virtual DbSet<Materialsupplier> Materialsuppliers { get; set; }

    public virtual DbSet<Materialtype> Materialtypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productcosthistory> Productcosthistories { get; set; }

    public virtual DbSet<Productmaterial> Productmaterials { get; set; }

    public virtual DbSet<Productsale> Productsales { get; set; }

    public virtual DbSet<Producttype> Producttypes { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=demo;UserName=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("agent_pkey");

            entity.ToTable("agent", "public3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Agenttypeid).HasColumnName("agenttypeid");
            entity.Property(e => e.Directorname).HasColumnName("directorname");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Inn).HasColumnName("inn");
            entity.Property(e => e.Kpp).HasColumnName("kpp");
            entity.Property(e => e.Logo).HasColumnName("logo");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.Title).HasColumnName("title");

            entity.HasOne(d => d.Agenttype).WithMany(p => p.Agents)
                .HasForeignKey(d => d.Agenttypeid)
                .HasConstraintName("agent_agenttypeid_fkey");
        });

        modelBuilder.Entity<Agentpriorityhistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("agentpriorityhistory_pkey");

            entity.ToTable("agentpriorityhistory", "public3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Agentid).HasColumnName("agentid");
            entity.Property(e => e.Changedate).HasColumnName("changedate");
            entity.Property(e => e.Priorityvalue).HasColumnName("priorityvalue");

            entity.HasOne(d => d.Agent).WithMany(p => p.Agentpriorityhistories)
                .HasForeignKey(d => d.Agentid)
                .HasConstraintName("agentpriorityhistory_agentid_fkey");
        });

        modelBuilder.Entity<Agenttype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("agenttype_pkey");

            entity.ToTable("agenttype", "public3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("material_pkey");

            entity.ToTable("material", "public3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost)
                .HasPrecision(10, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Countinpack).HasColumnName("countinpack");
            entity.Property(e => e.Countinstock)
                .HasPrecision(10, 2)
                .HasColumnName("countinstock");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Materialtypeid).HasColumnName("materialtypeid");
            entity.Property(e => e.Mincount)
                .HasPrecision(10, 2)
                .HasColumnName("mincount");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Unit).HasColumnName("unit");

            entity.HasOne(d => d.Materialtype).WithMany(p => p.Materials)
                .HasForeignKey(d => d.Materialtypeid)
                .HasConstraintName("material_materialtypeid_fkey");
        });

        modelBuilder.Entity<Materialcounthistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("materialcounthistory_pkey");

            entity.ToTable("materialcounthistory", "public3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Changedate).HasColumnName("changedate");
            entity.Property(e => e.Countvalue)
                .HasPrecision(10, 2)
                .HasColumnName("countvalue");
            entity.Property(e => e.Materialid).HasColumnName("materialid");

            entity.HasOne(d => d.Material).WithMany(p => p.Materialcounthistories)
                .HasForeignKey(d => d.Materialid)
                .HasConstraintName("materialcounthistory_materialid_fkey");
        });

        modelBuilder.Entity<Materialsupplier>(entity =>
        {
            entity.HasKey(e => e.Materialsupplierid).HasName("materialsupplier_pkey");

            entity.ToTable("materialsupplier", "public3");

            entity.Property(e => e.Materialsupplierid).HasColumnName("materialsupplierid");
            entity.Property(e => e.Materialid).HasColumnName("materialid");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");

            entity.HasOne(d => d.Material).WithMany(p => p.Materialsuppliers)
                .HasForeignKey(d => d.Materialid)
                .HasConstraintName("materialsupplier_materialid_fkey");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Materialsuppliers)
                .HasForeignKey(d => d.Supplierid)
                .HasConstraintName("materialsupplier_supplierid_fkey");
        });

        modelBuilder.Entity<Materialtype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("materialtype_pkey");

            entity.ToTable("materialtype", "public3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Defectedpercent)
                .HasPrecision(10, 2)
                .HasColumnName("defectedpercent");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product", "public3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Articlenumber).HasColumnName("articlenumber");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Mincostforagent)
                .HasPrecision(10, 2)
                .HasColumnName("mincostforagent");
            entity.Property(e => e.Productionpersoncount).HasColumnName("productionpersoncount");
            entity.Property(e => e.Productionworkshopnumber).HasColumnName("productionworkshopnumber");
            entity.Property(e => e.Producttypeid).HasColumnName("producttypeid");
            entity.Property(e => e.Title).HasColumnName("title");

            entity.HasOne(d => d.Producttype).WithMany(p => p.Products)
                .HasForeignKey(d => d.Producttypeid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("product_producttypeid_fkey");
        });

        modelBuilder.Entity<Productcosthistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productcosthistory_pkey");

            entity.ToTable("productcosthistory", "public3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Changedate).HasColumnName("changedate");
            entity.Property(e => e.Costvalue)
                .HasPrecision(10, 2)
                .HasColumnName("costvalue");
            entity.Property(e => e.Productid).HasColumnName("productid");

            entity.HasOne(d => d.Product).WithMany(p => p.Productcosthistories)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("productcosthistory_productid_fkey");
        });

        modelBuilder.Entity<Productmaterial>(entity =>
        {
            entity.HasKey(e => e.Productmaterialid).HasName("productmaterial_pkey");

            entity.ToTable("productmaterial", "public3");

            entity.Property(e => e.Productmaterialid).HasColumnName("productmaterialid");
            entity.Property(e => e.Count)
                .HasPrecision(10, 2)
                .HasColumnName("count");
            entity.Property(e => e.Materialid).HasColumnName("materialid");
            entity.Property(e => e.Productid).HasColumnName("productid");

            entity.HasOne(d => d.Material).WithMany(p => p.Productmaterials)
                .HasForeignKey(d => d.Materialid)
                .HasConstraintName("productmaterial_materialid_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.Productmaterials)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("productmaterial_productid_fkey");
        });

        modelBuilder.Entity<Productsale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productsale_pkey");

            entity.ToTable("productsale", "public3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Agentid).HasColumnName("agentid");
            entity.Property(e => e.Productcount).HasColumnName("productcount");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Saledate).HasColumnName("saledate");

            entity.HasOne(d => d.Agent).WithMany(p => p.Productsales)
                .HasForeignKey(d => d.Agentid)
                .HasConstraintName("productsale_agentid_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.Productsales)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("productsale_productid_fkey");
        });

        modelBuilder.Entity<Producttype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("producttype_pkey");

            entity.ToTable("producttype", "public3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Defectedpercent)
                .HasPrecision(10, 2)
                .HasColumnName("defectedpercent");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("shop_pkey");

            entity.ToTable("shop", "public3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Agentid).HasColumnName("agentid");
            entity.Property(e => e.Title).HasColumnName("title");

            entity.HasOne(d => d.Agent).WithMany(p => p.Shops)
                .HasForeignKey(d => d.Agentid)
                .HasConstraintName("shop_agentid_fkey");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("supplier_pkey");

            entity.ToTable("supplier", "public3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Inn).HasColumnName("inn");
            entity.Property(e => e.Qualityrating).HasColumnName("qualityrating");
            entity.Property(e => e.Startdate).HasColumnName("startdate");
            entity.Property(e => e.Suppliertype).HasColumnName("suppliertype");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
