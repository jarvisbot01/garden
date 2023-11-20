using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repository;

public class EmpleadoRepository : GenericRepository<Empleado>, IEmpleado
{
    private readonly GardenContext _context;

    public EmpleadoRepository(GardenContext context)
        : base(context)
    {
        _context = context;
    }
}
