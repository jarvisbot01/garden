using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ProductoController : BaseApiController
{
    private readonly IUnitOfWork _unitofwork;
    private readonly IMapper _mapper;

    public ProductoController(IUnitOfWork unitofwork, IMapper mapper)
    {
        _unitofwork = unitofwork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
    {
        var producto = await _unitofwork.Productos.GetAllAsync();
        return _mapper.Map<List<ProductoDto>>(producto);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoDto>> Get(int id)
    {
        var producto = await _unitofwork.Productos.GetByIdAsync(id);
        if (producto == null)
        {
            return NotFound();
        }
        return _mapper.Map<ProductoDto>(producto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Producto>> Post(ProductoDto ProductoDto)
    {
        var producto = _mapper.Map<Producto>(ProductoDto);
        _unitofwork.Productos.Add(producto);
        await _unitofwork.SaveAsync();
        if (producto == null)
        {
            return BadRequest();
        }
        ProductoDto.CodigoProducto = producto.CodigoProducto;
        return CreatedAtAction(nameof(Post), new { id = ProductoDto.CodigoProducto }, ProductoDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoDto>> Put(int id, [FromBody] ProductoDto ProductoDto)
    {
        if (ProductoDto == null)
        {
            return NotFound();
        }
        var producto = _mapper.Map<Producto>(ProductoDto);
        _unitofwork.Productos.Update(producto);
        await _unitofwork.SaveAsync();
        return ProductoDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _unitofwork.Productos.GetByIdAsync(id);
        if (producto == null)
        {
            return NotFound();
        }
        _unitofwork.Productos.Remove(producto);
        await _unitofwork.SaveAsync();
        return NoContent();
    }
}
