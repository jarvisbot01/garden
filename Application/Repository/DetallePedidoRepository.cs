using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repository;

public class DetallePedidoRepository : GenericRepository<DetallePedido>, IDetallePedido
{
    private readonly GardenContext _context;

    public DetallePedidoRepository(GardenContext context)
        : base(context)
    {
        _context = context;
    }
}
