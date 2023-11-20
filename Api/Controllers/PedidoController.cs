using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PedidoController : BaseApiController
{
    private readonly IUnitOfWork _unitofwork;
    private readonly IMapper _mapper;

    public PedidoController(IUnitOfWork unitofwork, IMapper mapper)
    {
        _unitofwork = unitofwork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PedidoDto>>> Get()
    {
        var pedido = await _unitofwork.Pedidos.GetAllAsync();
        return _mapper.Map<List<PedidoDto>>(pedido);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PedidoDto>> Get(int id)
    {
        var pedido = await _unitofwork.Pedidos.GetByIdAsync(id);
        if (pedido == null)
        {
            return NotFound();
        }
        return _mapper.Map<PedidoDto>(pedido);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pedido>> Post(PedidoDto PedidoDto)
    {
        var pedido = _mapper.Map<Pedido>(PedidoDto);
        _unitofwork.Pedidos.Add(pedido);
        await _unitofwork.SaveAsync();
        if (pedido == null)
        {
            return BadRequest();
        }
        PedidoDto.CodigoCliente = pedido.CodigoPedido;
        return CreatedAtAction(nameof(Post), new { id = PedidoDto.CodigoPedido }, PedidoDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PedidoDto>> Put(int id, [FromBody] PedidoDto PedidoDto)
    {
        if (PedidoDto == null)
        {
            return NotFound();
        }
        var pedido = _mapper.Map<Pedido>(PedidoDto);
        _unitofwork.Pedidos.Update(pedido);
        await _unitofwork.SaveAsync();
        return PedidoDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var pedido = await _unitofwork.Pedidos.GetByIdAsync(id);
        if (pedido == null)
        {
            return NotFound();
        }
        _unitofwork.Pedidos.Remove(pedido);
        await _unitofwork.SaveAsync();
        return NoContent();
    }
}
