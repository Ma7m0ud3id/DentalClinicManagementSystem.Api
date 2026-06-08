namespace DentalClinicManagementSystem.DAL.Seed;

using BCrypt.Net;
using DentalClinicManagementSystem.DAL.Data;
using DentalClinicManagementSystem.DAL.Entities;
using DentalClinicManagementSystem.DAL.Enums;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.Users.Any())
        {
            return;
        }

        var users = new List<User>
        {
            new User
            {
                Username = "admin",
                PasswordHash = BCrypt.HashPassword("Admin@123"),
                FullName = "System Administrator",
                Phone = "01000000001",
                Role = UserRole.Admin,
                Specialization = null,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Username = "doctor1",
                PasswordHash = BCrypt.HashPassword("Doctor@123"),
                FullName = "Dr. Ahmed Hassan",
                Phone = "01000000002",
                Role = UserRole.Doctor,
                Specialization = "General Dentist",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Username = "recep1",
                PasswordHash = BCrypt.HashPassword("Recep@123"),
                FullName = "Sara Mohamed",
                Phone = "01000000003",
                Role = UserRole.Receptionist,
                Specialization = null,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }
}