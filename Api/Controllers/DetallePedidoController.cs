using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class DetallePedidoController : BaseApiController
{
    private readonly IUnitOfWork _unitofwork;
    private readonly IMapper _mapper;

    public DetallePedidoController(IUnitOfWork unitofwork, IMapper mapper)
    {
        _unitofwork = unitofwork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<DetallePedidoDto>>> Get()
    {
        var detallePedido = await _unitofwork.DetallePedidos.GetAllAsync();
        return _mapper.Map<List<DetallePedidoDto>>(detallePedido);
    }

    [HttpGet("{CodigoPedido}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DetallePedidoDto>> Get(int CodigoPedido)
    {
        var detallePedido = await _unitofwork.DetallePedidos.GetByIdAsync(CodigoPedido);
        if (detallePedido == null)
        {
            return NotFound();
        }
        return _mapper.Map<DetallePedidoDto>(detallePedido);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DetallePedido>> Post(DetallePedidoDto DetallePedidoDto)
    {
        var detallePedido = _mapper.Map<DetallePedido>(DetallePedidoDto);
        _unitofwork.DetallePedidos.Add(detallePedido);
        await _unitofwork.SaveAsync();
        if (detallePedido == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), DetallePedidoDto);
    }

    [HttpPut("{CodigoPedido}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DetallePedidoDto>> Put(
        int CodigoPedido,
        [FromBody] DetallePedidoDto DetallePedidoDto
    )
    {
        if (DetallePedidoDto == null)
        {
            return NotFound();
        }
        var detallePedido = _mapper.Map<DetallePedido>(DetallePedidoDto);
        _unitofwork.DetallePedidos.Update(detallePedido);
        await _unitofwork.SaveAsync();
        return DetallePedidoDto;
    }

    [HttpDelete("{CodigoPedido}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int CodigoPedido)
    {
        var detallePedido = await _unitofwork.DetallePedidos.GetByIdAsync(CodigoPedido);
        if (detallePedido == null)
        {
            return NotFound();
        }
        _unitofwork.DetallePedidos.Remove(detallePedido);
        await _unitofwork.SaveAsync();
        return NoContent();
    }
}
