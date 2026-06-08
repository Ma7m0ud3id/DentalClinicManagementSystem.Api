namespace DentalClinicManagementSystem.DAL.Configurations;

using DentalClinicManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.ToTable("Visits");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.AppointmentId)
            .IsRequired();
        builder.HasIndex(v => v.AppointmentId)
            .IsUnique();

        builder.Property(v => v.PatientId)
            .IsRequired();

        builder.Property(v => v.DoctorId)
            .IsRequired();

        builder.Property(v => v.Diagnosis)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(v => v.Treatment)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(v => v.DoctorNotes)
            .HasMaxLength(1000);

        builder.Property(v => v.VisitDate)
            .IsRequired();

        builder.Property(v => v.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(v => v.Appointment)
            .WithOne(a => a.Visit)
            .HasForeignKey<Visit>(v => v.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.Patient)
            .WithMany(p => p.Visits)
            .HasForeignKey(v => v.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.Doctor)
            .WithMany(u => u.DoctorVisits)
            .HasForeignKey(v => v.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}