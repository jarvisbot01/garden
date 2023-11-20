using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repository;

public class PagoRepository : GenericRepository<Pago>, IPago
{
    private readonly GardenContext _context;

    public PagoRepository(GardenContext context)
        : base(context)
    {
        _context = context;
    }
}
