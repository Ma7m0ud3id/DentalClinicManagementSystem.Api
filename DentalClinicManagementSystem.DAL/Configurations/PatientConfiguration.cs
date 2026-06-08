namespace DentalClinicManagementSystem.DAL.Configurations;

using DentalClinicManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.Gender)
            .IsRequired();

        builder.Property(p => p.Address)
            .HasMaxLength(200);

        builder.Property(p => p.MedicalNotes)
            .HasMaxLength(1000);

        builder.Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        // Authentication properties
        builder.Property(p => p.Username)
            .HasMaxLength(50);

        builder.HasIndex(p => p.Username)
            .IsUnique()
            .HasFilter("[Username] IS NOT NULL");

        builder.Property(p => p.PasswordHash)
            .HasMaxLength(500);

        builder.Property(p => p.Email)
            .HasMaxLength(100);

        // Relationships
        builder.HasMany(p => p.Appointments)
            .WithOne(a => a.Patient)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Visits)
            .WithOne(v => v.Patient)
            .HasForeignKey(v => v.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}