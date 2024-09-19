using System;
using System.Collections.Generic;
using GranjaLosAres_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GranjaLosAres_API.Data;

public partial class MyDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration)
    : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<ClasificacionHuevo> ClasificacionHuevos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Corral> Corrals { get; set; }

    public virtual DbSet<DetallesVentum> DetallesVenta { get; set; }

    public virtual DbSet<EstadoLote> EstadoLotes { get; set; }

    public virtual DbSet<Etapa> Etapas { get; set; }

    public virtual DbSet<Lote> Lotes { get; set; }

    public virtual DbSet<ProduccionGallina> ProduccionGallinas { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<RazaGallina> RazaGallinas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StockHuevo> StockHuevos { get; set; }

    public virtual DbSet<TriggerDebugLog> TriggerDebugLogs { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<VistaClasificacionHuevo> VistaClasificacionHuevos { get; set; }

    public virtual DbSet<VistaDashboard> VistaDashboards { get; set; }

    public virtual DbSet<VistaStockRestanteHuevo> VistaStockRestanteHuevos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("GranjaAres1Database");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClasificacionHuevo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clasific__3214EC27BBDCC8F0");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FechaClaS).HasColumnType("datetime");
            entity.Property(e => e.Tamano)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TotalUnitaria).HasComputedColumnSql("([dbo].[CalcularCantidadTotalUnitaria]([Cajas],[CartonesExtras],[HuevosSueltos]))", false);

            entity.HasOne(d => d.IdProdNavigation).WithMany(p => p.ClasificacionHuevos)
                .HasForeignKey(d => d.IdProd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClasificacionHuevos_ProduccionGallinas");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD0A7E7862C51");

            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Corral>(entity =>
        {
            entity.HasKey(e => e.IdCorral);

            entity.ToTable("Corral");

            entity.Property(e => e.IdCorral).ValueGeneratedNever();
            entity.Property(e => e.NumCorral).HasMaxLength(50);
        });

        modelBuilder.Entity<DetallesVentum>(entity =>
        {
            entity.HasKey(e => e.DetalleId).HasName("PK__Detalles__6E19D6FA2008A5B1");

            entity.Property(e => e.DetalleId).HasColumnName("DetalleID");
            entity.Property(e => e.CantidadVendida).HasDefaultValue(0);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.PrecioUnitario)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.TamanoHuevo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoEmpaque)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Total)
                .HasComputedColumnSql("([CantidadVendida]*[PrecioUnitario])", true)
                .HasColumnType("decimal(21, 2)");
            entity.Property(e => e.TotalHuevos).HasComputedColumnSql("(case when [TipoEmpaque]='Caja' then [CantidadVendida]*(360) when [TipoEmpaque]='Cartón' then [CantidadVendida]*(30) else [CantidadVendida] end)", true);
            entity.Property(e => e.VentaId).HasColumnName("VentaID");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetallesVenta)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("FK__DetallesV__Produ__59FA5E80");

            entity.HasOne(d => d.Venta).WithMany(p => p.DetallesVenta)
                .HasForeignKey(d => d.VentaId)
                .HasConstraintName("FK__DetallesV__Venta__5BE2A6F2");
        });

        modelBuilder.Entity<EstadoLote>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK_EstadoLote_G");

            entity.ToTable("EstadoLote");

            entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
            entity.Property(e => e.CantidadG).HasColumnName("Cantidad_G");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Registro");
            entity.Property(e => e.IdEtapa).HasColumnName("Id_Etapa");

            entity.HasOne(d => d.IdEtapaNavigation).WithMany(p => p.EstadoLotes)
                .HasForeignKey(d => d.IdEtapa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EstadoLote_Etapas");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.EstadoLotes)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EstadoLote_Lote");
        });

        modelBuilder.Entity<Etapa>(entity =>
        {
            entity.HasKey(e => e.IdEtapa);

            entity.Property(e => e.IdEtapa).ValueGeneratedNever();
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Lote>(entity =>
        {
            entity.HasKey(e => e.IdLote).HasName("PK_Table1");

            entity.ToTable("Lote");

            entity.Property(e => e.IdLote).ValueGeneratedNever();
            entity.Property(e => e.FechaAdq).HasColumnType("datetime");
            entity.Property(e => e.NumLote)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCorralNavigation).WithMany(p => p.Lotes)
                .HasForeignKey(d => d.IdCorral)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Table1_Corral");

            entity.HasOne(d => d.IdRazaNavigation).WithMany(p => p.Lotes)
                .HasForeignKey(d => d.IdRaza)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Table1_RazaGallina");
        });

        modelBuilder.Entity<ProduccionGallina>(entity =>
        {
            entity.HasKey(e => e.IdProd).HasName("PK__Producci__E40D971DC3C087DC");

            entity.Property(e => e.CantTotal).HasComputedColumnSql("([dbo].[CalcularCantidadTotalProduccion]([CantCajas],[CantCartones],[CantSueltos]))", false);
            entity.Property(e => e.Defectuosos).HasDefaultValue(0);
            entity.Property(e => e.FechaRegistroP).HasColumnType("datetime");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.ProduccionGallinas)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProduccionGallinas_Lote");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AE83E9DF4B6A");

            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RazaGallina>(entity =>
        {
            entity.HasKey(e => e.IdRaza);

            entity.ToTable("RazaGallina");

            entity.Property(e => e.IdRaza).ValueGeneratedNever();
            entity.Property(e => e.CaractEspec)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(90)
                .IsUnicode(false);
            entity.Property(e => e.ColorH)
                .HasMaxLength(90)
                .IsUnicode(false);
            entity.Property(e => e.Origen)
                .HasMaxLength(90)
                .IsUnicode(false);
            entity.Property(e => e.Raza)
                .HasMaxLength(90)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC071BC0321D");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<StockHuevo>(entity =>
        {
            entity.HasKey(e => e.Tamano).HasName("PK__StockHue__799ADF87B8CE711C");

            entity.ToTable(tb => tb.HasTrigger("trg_ResetStockHuevos"));

            entity.Property(e => e.Tamano).HasMaxLength(50);
        });

        modelBuilder.Entity<TriggerDebugLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TriggerDebugLog");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.OperationType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC07059D3025");

            entity.Property(e => e.Contrasena).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaDeRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreUser).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Usuarios__RoleId__628FA481");
        });



        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.VentaId).HasName("PK__Ventas__5B41514CA5140F29");

            entity.Property(e => e.VentaId).HasColumnName("VentaID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.TotalVenta).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Venta)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK__Ventas__ClienteI__6477ECF3");
        });

        modelBuilder.Entity<VistaClasificacionHuevo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaClasificacionHuevos");

            entity.Property(e => e.Tamaño)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VistaDashboard>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaDashboard");

            entity.Property(e => e.Raza)
                .HasMaxLength(90)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VistaStockRestanteHuevo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaStockRestanteHuevos");

            entity.Property(e => e.FechaProdu).HasColumnType("datetime");
            entity.Property(e => e.IdProduccion).ValueGeneratedOnAdd();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
