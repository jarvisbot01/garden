using Api.Dtos;
using Api.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UserController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitofwork;
    private readonly IMapper _mapper;

    public UserController(IUserService UserService, IUnitOfWork unitofwork, IMapper mapper)
    {
        _unitofwork = unitofwork;
        _mapper = mapper;
        _userService = UserService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get()
    {
        var users = await _unitofwork.Users.GetAllAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> Get(int id)
    {
        var user = await _unitofwork.Users.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return _mapper.Map<UserDto>(user);
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(RegisterDto model)
    {
        var result = await _userService.RegisterAsync(model);
        return Ok(result);
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(LoginDto model)
    {
        var result = await _userService.GetTokenAsync(model);
        SetRefreshTokenInCookie(result.RefreshToken);
        return Ok(result);
    }

    [HttpPost("addrole")]
    public async Task<IActionResult> AddRoleAsync(AddRoleDto model)
    {
        var result = await _userService.AddRoleAsync(model);
        return Ok(result);
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _userService.RefreshTokenAsync(refreshToken);
        if (!string.IsNullOrEmpty(response.RefreshToken))
            SetRefreshTokenInCookie(response.RefreshToken);
        return Ok(response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> Put(int id, [FromBody] UserDto userDto)
    {
        if (userDto == null)
        {
            return NotFound();
        }
        var user = _mapper.Map<User>(userDto);
        _unitofwork.Users.Update(user);
        await _unitofwork.SaveAsync();
        return userDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _unitofwork.Users.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        _unitofwork.Users.Remove(user);
        await _unitofwork.SaveAsync();
        return NoContent();
    }

    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(1),
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}
