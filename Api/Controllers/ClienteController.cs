using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Roles = "Employee")]
public class ClienteController : BaseApiController
{
    private readonly IUnitOfWork _unitofwork;
    private readonly IMapper _mapper;

    public ClienteController(IUnitOfWork unitofwork, IMapper mapper)
    {
        _unitofwork = unitofwork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> Get()
    {
        var cliente = await _unitofwork.Clientes.GetAllAsync();
        return _mapper.Map<List<ClienteDto>>(cliente);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteDto>> Get(int id)
    {
        var cliente = await _unitofwork.Clientes.GetByIdAsync(id);
        if (cliente == null)
        {
            return NotFound();
        }
        return _mapper.Map<ClienteDto>(cliente);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Cliente>> Post(ClienteDto ClienteDto)
    {
        var cliente = _mapper.Map<Cliente>(ClienteDto);
        _unitofwork.Clientes.Add(cliente);
        await _unitofwork.SaveAsync();
        if (cliente == null)
        {
            return BadRequest();
        }
        ClienteDto.CodigoCliente = cliente.CodigoCliente;
        return CreatedAtAction(nameof(Post), new { id = ClienteDto.CodigoCliente }, ClienteDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteDto>> Put(int id, [FromBody] ClienteDto ClienteDto)
    {
        if (ClienteDto == null)
        {
            return NotFound();
        }
        var cliente = _mapper.Map<Cliente>(ClienteDto);
        _unitofwork.Clientes.Update(cliente);
        await _unitofwork.SaveAsync();
        return ClienteDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _unitofwork.Clientes.GetByIdAsync(id);
        if (cliente == null)
        {
            return NotFound();
        }
        _unitofwork.Clientes.Remove(cliente);
        await _unitofwork.SaveAsync();
        return NoContent();
    }
}
