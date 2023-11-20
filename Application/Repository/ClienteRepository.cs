using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repository;

public class ClienteRepository : GenericRepository<Cliente>, ICliente
{
    private readonly GardenContext _context;

    public ClienteRepository(GardenContext context)
        : base(context)
    {
        _context = context;
    }
}
