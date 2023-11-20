using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class EmpleadoController : BaseApiController
{
    private readonly IUnitOfWork _unitofwork;
    private readonly IMapper _mapper;

    public EmpleadoController(IUnitOfWork unitofwork, IMapper mapper)
    {
        _unitofwork = unitofwork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<EmpleadoDto>>> Get()
    {
        var empleado = await _unitofwork.Empleados.GetAllAsync();
        return _mapper.Map<List<EmpleadoDto>>(empleado);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmpleadoDto>> Get(int id)
    {
        var empleado = await _unitofwork.Empleados.GetByIdAsync(id);
        if (empleado == null)
        {
            return NotFound();
        }
        return _mapper.Map<EmpleadoDto>(empleado);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Empleado>> Post(EmpleadoDto EmpleadoDto)
    {
        var empleado = _mapper.Map<Empleado>(EmpleadoDto);
        _unitofwork.Empleados.Add(empleado);
        await _unitofwork.SaveAsync();
        if (empleado == null)
        {
            return BadRequest();
        }
        EmpleadoDto.CodigoEmpleado = empleado.CodigoEmpleado;
        return CreatedAtAction(nameof(Post), new { id = EmpleadoDto.CodigoEmpleado }, EmpleadoDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmpleadoDto>> Put(int id, [FromBody] EmpleadoDto EmpleadoDto)
    {
        if (EmpleadoDto == null)
        {
            return NotFound();
        }
        var empleado = _mapper.Map<Empleado>(EmpleadoDto);
        _unitofwork.Empleados.Update(empleado);
        await _unitofwork.SaveAsync();
        return EmpleadoDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var empleado = await _unitofwork.Empleados.GetByIdAsync(id);
        if (empleado == null)
        {
            return NotFound();
        }
        _unitofwork.Empleados.Remove(empleado);
        await _unitofwork.SaveAsync();
        return NoContent();
    }
}
