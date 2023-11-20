using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class OficinaController : BaseApiController
{
    private readonly IUnitOfWork _unitofwork;
    private readonly IMapper _mapper;

    public OficinaController(IUnitOfWork unitofwork, IMapper mapper)
    {
        _unitofwork = unitofwork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<OficinaDto>>> Get()
    {
        var oficina = await _unitofwork.Oficinas.GetAllAsync();
        return _mapper.Map<List<OficinaDto>>(oficina);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OficinaDto>> Get(int id)
    {
        var oficina = await _unitofwork.Oficinas.GetByIdAsync(id);
        if (oficina == null)
        {
            return NotFound();
        }
        return _mapper.Map<OficinaDto>(oficina);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Oficina>> Post(OficinaDto OficinaDto)
    {
        var oficina = _mapper.Map<Oficina>(OficinaDto);
        _unitofwork.Oficinas.Add(oficina);
        await _unitofwork.SaveAsync();
        if (oficina == null)
        {
            return BadRequest();
        }
        OficinaDto.CodigoOficina = oficina.CodigoOficina;
        return CreatedAtAction(nameof(Post), new { id = OficinaDto.CodigoOficina }, OficinaDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OficinaDto>> Put(int id, [FromBody] OficinaDto OficinaDto)
    {
        if (OficinaDto == null)
        {
            return NotFound();
        }
        var oficina = _mapper.Map<Oficina>(OficinaDto);
        _unitofwork.Oficinas.Update(oficina);
        await _unitofwork.SaveAsync();
        return OficinaDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var oficina = await _unitofwork.Oficinas.GetByIdAsync(id);
        if (oficina == null)
        {
            return NotFound();
        }
        _unitofwork.Oficinas.Remove(oficina);
        await _unitofwork.SaveAsync();
        return NoContent();
    }
}
