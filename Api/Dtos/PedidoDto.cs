namespace Api.Dtos;

public class PedidoDto
{
    public int CodigoPedido { get; set; }

    public DateOnly FechaPedido { get; set; }

    public DateOnly FechaEsperada { get; set; }

    public DateOnly FechaEntrega { get; set; }

    public string Estado { get; set; }

    public string Comentarios { get; set; }

    public int CodigoCliente { get; set; }
}
