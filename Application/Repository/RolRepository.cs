using Domain.Entities;
using Domain.Interfaces;
using Persistence;
using Persistence.Data;

namespace Application.Repository;

public class RolRepository : GenericRepository<Rol>, IRol
{
    private readonly GardenContext _context;

    public RolRepository(GardenContext context)
        : base(context)
    {
        _context = context;
    }
}
