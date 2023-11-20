using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repository;

public class ProductoRepository : GenericRepository<Producto>, IProducto
{
    private readonly GardenContext _context;

    public ProductoRepository(GardenContext context)
        : base(context)
    {
        _context = context;
    }
}
