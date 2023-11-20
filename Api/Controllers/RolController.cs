using Api.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "Employee")]
    public class RolController : BaseApiController
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;

        public RolController(IUnitOfWork unitofwork, IMapper mapper)
        {
            _unitofwork = unitofwork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<RolDto>>> Get()
        {
            var rol = await _unitofwork.Roles.GetAllAsync();
            return _mapper.Map<List<RolDto>>(rol);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RolDto>> Get(int id)
        {
            var rol = await _unitofwork.Roles.GetByIdAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            return _mapper.Map<RolDto>(rol);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Rol>> Post(RolDto rolDto)
        {
            var rol = _mapper.Map<Rol>(rolDto);
            _unitofwork.Roles.Add(rol);
            await _unitofwork.SaveAsync();
            if (rol == null)
            {
                return BadRequest();
            }
            rolDto.Id = rol.Id;
            return CreatedAtAction(nameof(Post), new { id = rolDto.Id }, rolDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RolDto>> Put(int id, [FromBody] RolDto rolDto)
        {
            if (rolDto == null)
            {
                return NotFound();
            }
            var rol = _mapper.Map<Rol>(rolDto);
            _unitofwork.Roles.Update(rol);
            await _unitofwork.SaveAsync();
            return rolDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var rol = await _unitofwork.Roles.GetByIdAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            _unitofwork.Roles.Remove(rol);
            await _unitofwork.SaveAsync();
            return NoContent();
        }
    }
}
