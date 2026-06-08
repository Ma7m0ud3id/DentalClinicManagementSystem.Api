using DentalClinicManagementSystem.BLL.CurrentUser;
using DentalClinicManagementSystem.BLL.DTOs.Users;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinicManagementSystem.Apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;

        public UsersController(IUserService userService, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll([FromQuery] int? role, [FromQuery] bool? isActive)
        {
            var users = await _userService.GetAllAsync(role, isActive);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found" });
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
        {
            var result = await _userService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(int id, UpdateUserDto dto)
        {
            var result = await _userService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpPatch("{id}/toggle-status")]
        public async Task<ActionResult<UserDto>> ToggleStatus(int id)
        {
            var currentUserId = _currentUserService.UserId;
            if (!currentUserId.HasValue)
            {
                return Unauthorized();
            }

            var result = await _userService.ToggleStatusAsync(id, currentUserId.Value);
            return Ok(result);
        }

        [HttpPatch("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordDto dto)
        {
            await _userService.ChangePasswordAsync(id, dto);
            return NoContent();
        }
    }
}