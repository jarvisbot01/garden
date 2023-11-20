namespace Api.Dtos;

public class ProductoDto
{
    public string CodigoProducto { get; set; }

    public string Nombre { get; set; }

    public string Gama { get; set; }

    public string Dimensiones { get; set; }

    public string Proveedor { get; set; }

    public string Descripcion { get; set; }

    public short CantidadEnStock { get; set; }

    public decimal PrecioVenta { get; set; }

    public decimal PrecioProveedor { get; set; }
}
