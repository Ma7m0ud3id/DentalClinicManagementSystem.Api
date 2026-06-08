namespace DentalClinicManagementSystem.BLL.CurrentUser;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    int? UserId { get; }
    string? Username { get; }
    string? FullName { get; }
    string? Role { get; }
    string? UserType { get; }
}