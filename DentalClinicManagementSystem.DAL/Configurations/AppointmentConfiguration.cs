namespace DentalClinicManagementSystem.DAL.Configurations;

using DentalClinicManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.PatientId)
            .IsRequired();

        builder.Property(a => a.DoctorId)
            .IsRequired();

        builder.Property(a => a.AppointmentDate)
            .IsRequired();

        builder.Property(a => a.DurationMinutes)
            .IsRequired()
            .HasDefaultValue(30);

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(a => a.Notes)
            .HasMaxLength(500);

        builder.Property(a => a.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Doctor)
            .WithMany(u => u.DoctorAppointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.AppointmentDate);
        builder.HasIndex(a => a.DoctorId);
        builder.HasIndex(a => a.PatientId);
    }
}