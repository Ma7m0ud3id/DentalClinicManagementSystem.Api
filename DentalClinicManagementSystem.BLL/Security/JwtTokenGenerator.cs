namespace DentalClinicManagementSystem.BLL.Security;

using DentalClinicManagementSystem.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class JwtTokenGenerator
{
    public static string GenerateToken(User user, JwtSettings settings)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim("username", user.Username),
            new Claim("userType", "Staff"),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(settings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(settings.ExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string GeneratePatientToken(Patient patient, JwtSettings settings)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, patient.Id.ToString()),
            new Claim(ClaimTypes.Name, patient.FullName),
            new Claim("username", patient.Username ?? string.Empty),
            new Claim("userType", "Patient"),
            new Claim(ClaimTypes.Role, "Patient")
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(settings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(settings.ExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}