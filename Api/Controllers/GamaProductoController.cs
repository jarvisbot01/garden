using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class GamaProductoController : BaseApiController
{
    private readonly IUnitOfWork _unitofwork;
    private readonly IMapper _mapper;

    public GamaProductoController(IUnitOfWork unitofwork, IMapper mapper)
    {
        _unitofwork = unitofwork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<GamaProductoDto>>> Get()
    {
        var gamaProducto = await _unitofwork.GamaProductos.GetAllAsync();
        return _mapper.Map<List<GamaProductoDto>>(gamaProducto);
    }

    [HttpGet("{Gama}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GamaProductoDto>> Get(string Gama)
    {
        var gamaProducto = await _unitofwork.GamaProductos.GetByNameAsync(Gama);
        if (gamaProducto == null)
        {
            return NotFound();
        }
        return _mapper.Map<GamaProductoDto>(gamaProducto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GamaProducto>> Post(GamaProductoDto GamaProductoDto)
    {
        var gamaProducto = _mapper.Map<GamaProducto>(GamaProductoDto);
        _unitofwork.GamaProductos.Add(gamaProducto);
        await _unitofwork.SaveAsync();
        if (gamaProducto == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), GamaProductoDto);
    }

    [HttpPut("{Gama}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GamaProductoDto>> Put(
        string Gama,
        [FromBody] GamaProductoDto GamaProductoDto
    )
    {
        if (GamaProductoDto == null)
        {
            return NotFound();
        }
        var gamaProducto = _mapper.Map<GamaProducto>(GamaProductoDto);
        _unitofwork.GamaProductos.Update(gamaProducto);
        await _unitofwork.SaveAsync();
        return GamaProductoDto;
    }

    [HttpDelete("{Gama}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string Gama)
    {
        var gamaProducto = await _unitofwork.GamaProductos.GetByNameAsync(Gama);
        if (gamaProducto == null)
        {
            return NotFound();
        }
        _unitofwork.GamaProductos.Remove(gamaProducto);
        await _unitofwork.SaveAsync();
        return NoContent();
    }
}
