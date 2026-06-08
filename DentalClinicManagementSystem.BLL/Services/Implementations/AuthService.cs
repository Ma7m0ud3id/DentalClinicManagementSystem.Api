using DentalClinicManagementSystem.BLL.DTOs.Auth;
using DentalClinicManagementSystem.BLL.Security;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using DentalClinicManagementSystem.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DentalClinicManagementSystem.BLL.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IValidator<LoginRequestDto> _loginValidator;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        AppDbContext context,
        IValidator<LoginRequestDto> loginValidator,
        IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _loginValidator = loginValidator;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        // Validate input
        await _loginValidator.ValidateAndThrowAsync(dto);

        // Find user by username
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("User account is inactive");
        }

        // Generate token
        var token = JwtTokenGenerator.GenerateToken(user, _jwtSettings);

        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            UserType = "Staff"
        };
    }

    public async Task<CurrentUserDto?> GetCurrentUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            return null;

        return new CurrentUserDto
        {
            UserId = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role.ToString()
        };
    }
}