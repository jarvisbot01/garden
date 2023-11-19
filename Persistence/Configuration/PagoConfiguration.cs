using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration;

public class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {
        builder
            .HasKey(e => new { e.CodigoCliente, e.IdTransaccion })
            .HasName("PRIMARY")
            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

        builder.ToTable("pago");

        builder
            .Property(e => e.CodigoCliente)
            .HasColumnType("int(11)")
            .HasColumnName("codigo_cliente");
        builder.Property(e => e.IdTransaccion).HasMaxLength(50).HasColumnName("id_transaccion");
        builder.Property(e => e.FechaPago).HasColumnName("fecha_pago");
        builder
            .Property(e => e.FormaPago)
            .IsRequired()
            .HasMaxLength(48)
            .HasColumnName("forma_pago");
        builder.Property(e => e.Total).HasPrecision(15, 2).HasColumnName("total");

        builder
            .HasOne(d => d.CodigoClienteNavigation)
            .WithMany(p => p.Pagos)
            .HasForeignKey(d => d.CodigoCliente)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("pago_ibfk_1");
    }
}
