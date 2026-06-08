namespace DentalClinicManagementSystem.BLL.CurrentUser;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public int? UserId =>
        int.TryParse(
            _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            out var userId)
            ? userId
            : null;

    public string? Username =>
        _httpContextAccessor.HttpContext?.User.FindFirst("username")?.Value;

    public string? FullName =>
        _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;

    public string? Role =>
        _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

    public string? UserType =>
        _httpContextAccessor.HttpContext?.User.FindFirst("userType")?.Value;
}