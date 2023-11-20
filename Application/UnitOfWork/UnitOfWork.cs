using Application.Repository;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly GardenContext _context;
    private ClienteRepository _clienteRepository;
    private DetallePedidoRepository _detallePedidoRepository;
    private EmpleadoRepository _empleadoRepository;
    private GamaProductoRepository _gamaProductoRepository;
    private OficinaRepository _oficinaRepository;
    private PagoRepository _pagoRepository;
    private PedidoRepository _pedidoRepository;
    private ProductoRepository _productoRepository;

    public UnitOfWork(GardenContext context)
    {
        _context = context;
    }

    public ICliente Clientes
    {
        get
        {
            if (_clienteRepository == null)
            {
                _clienteRepository = new ClienteRepository(_context);
            }

            return _clienteRepository;
        }
    }

    public IDetallePedido DetallePedidos
    {
        get
        {
            if (_detallePedidoRepository == null)
            {
                _detallePedidoRepository = new DetallePedidoRepository(_context);
            }

            return _detallePedidoRepository;
        }
    }

    public IEmpleado Empleados
    {
        get
        {
            if (_empleadoRepository == null)
            {
                _empleadoRepository = new EmpleadoRepository(_context);
            }

            return _empleadoRepository;
        }
    }

    public IGamaProducto GamaProductos
    {
        get
        {
            if (_gamaProductoRepository == null)
            {
                _gamaProductoRepository = new GamaProductoRepository(_context);
            }

            return _gamaProductoRepository;
        }
    }

    public IOficina Oficinas
    {
        get
        {
            if (_oficinaRepository == null)
            {
                _oficinaRepository = new OficinaRepository(_context);
            }

            return _oficinaRepository;
        }
    }

    public IPago Pagos
    {
        get
        {
            if (_pagoRepository == null)
            {
                _pagoRepository = new PagoRepository(_context);
            }

            return _pagoRepository;
        }
    }

    public IPedido Pedidos
    {
        get
        {
            if (_pedidoRepository == null)
            {
                _pedidoRepository = new PedidoRepository(_context);
            }

            return _pedidoRepository;
        }
    }

    public IProducto Productos
    {
        get
        {
            if (_productoRepository == null)
            {
                _productoRepository = new ProductoRepository(_context);
            }

            return _productoRepository;
        }
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
