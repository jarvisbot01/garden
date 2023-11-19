using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data;

public partial class GardenContext : DbContext
{
    public GardenContext() { }

    public GardenContext(DbContextOptions<GardenContext> options)
        : base(options) { }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<GamaProducto> GamaProductos { get; set; }

    public virtual DbSet<Oficina> Oficinas { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");
    }
}
