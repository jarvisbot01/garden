namespace Domain.Entities;

public partial class Pago
{
    public int CodigoCliente { get; set; }

    public string FormaPago { get; set; }

    public string IdTransaccion { get; set; }

    public DateOnly FechaPago { get; set; }

    public decimal Total { get; set; }

    public virtual Cliente CodigoClienteNavigation { get; set; }
}
