using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repository;

public class OficinaRepository : GenericRepository<Oficina>, IOficina
{
    private readonly GardenContext _context;

    public OficinaRepository(GardenContext context)
        : base(context)
    {
        _context = context;
    }
}
