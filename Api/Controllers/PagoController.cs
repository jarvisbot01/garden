using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PagoController : BaseApiController
{
    private readonly IUnitOfWork _unitofwork;
    private readonly IMapper _mapper;

    public PagoController(IUnitOfWork unitofwork, IMapper mapper)
    {
        _unitofwork = unitofwork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PagoDto>>> Get()
    {
        var pago = await _unitofwork.Pagos.GetAllAsync();
        return _mapper.Map<List<PagoDto>>(pago);
    }

    [HttpGet("{codigoCliente}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagoDto>> Get(int CodigoPedido)
    {
        var pago = await _unitofwork.Pagos.GetByIdAsync(CodigoPedido);
        if (pago == null)
        {
            return NotFound();
        }
        return _mapper.Map<PagoDto>(pago);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pago>> Post(PagoDto PagoDto)
    {
        var pago = _mapper.Map<Pago>(PagoDto);
        _unitofwork.Pagos.Add(pago);
        await _unitofwork.SaveAsync();
        if (pago == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), PagoDto);
    }

    [HttpPut("{codigoCliente}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagoDto>> Put(int codigoCliente, [FromBody] PagoDto PagoDto)
    {
        if (PagoDto == null)
        {
            return NotFound();
        }
        var pago = _mapper.Map<Pago>(PagoDto);
        _unitofwork.Pagos.Update(pago);
        await _unitofwork.SaveAsync();
        return PagoDto;
    }

    [HttpDelete("{codigoCliente}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int CodigoPedido)
    {
        var pago = await _unitofwork.Pagos.GetByIdAsync(CodigoPedido);
        if (pago == null)
        {
            return NotFound();
        }
        _unitofwork.Pagos.Remove(pago);
        await _unitofwork.SaveAsync();
        return NoContent();
    }
}
