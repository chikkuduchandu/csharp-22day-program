using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CareBridge.PerformanceLab.Models;

public partial class CareBridgeContext : DbContext
{
    public CareBridgeContext()
    {
    }

    public CareBridgeContext(DbContextOptions<CareBridgeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Claim> Claims { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<Encounter> Encounters { get; set; }

    public virtual DbSet<Insurance> Insurances { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Procedure> Procedures { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<VwAnalyticsDeId> VwAnalyticsDeIds { get; set; }

    public virtual DbSet<VwClinical> VwClinicals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=CareBridgeDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Claim>(entity =>
        {
            entity.HasKey(e => e.ClaimId).HasName("PK__Claim__EF2E139BAEA98BB5");

            entity.ToTable("Claim");

            entity.Property(e => e.BilledAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ReimbursedAmt).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Encounter).WithMany(p => p.Claims)
                .HasForeignKey(d => d.EncounterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Claim__Encounter__5070F446");

            entity.HasOne(d => d.Insurance).WithMany(p => p.Claims)
                .HasForeignKey(d => d.InsuranceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Claim__Insurance__5165187F");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BEDB9656D36");

            entity.ToTable("Department");

            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.HasKey(e => e.DiagnosisId).HasName("PK__Diagnosi__0C54CC7307078698");

            entity.ToTable("Diagnosis");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.DiagnosedOn).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IcdCode).HasMaxLength(10);

            entity.HasOne(d => d.Encounter).WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.EncounterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Diagnosis__Encou__49C3F6B7");
        });

        modelBuilder.Entity<Encounter>(entity =>
        {
            entity.HasKey(e => e.EncounterId).HasName("PK__Encounte__4278DD3676E03F5A");

            entity.ToTable("Encounter");

            entity.HasIndex(e => e.EncounterType, "NCI-encountertype");

            entity.Property(e => e.EncounterType).HasMaxLength(30);

            entity.HasOne(d => d.Department).WithMany(p => p.Encounters)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Encounter__Depar__45F365D3");

            entity.HasOne(d => d.Patient).WithMany(p => p.Encounters)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Encounter__Patie__440B1D61");

            entity.HasOne(d => d.Provider).WithMany(p => p.Encounters)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK__Encounter__Provi__44FF419A");
        });

        modelBuilder.Entity<Insurance>(entity =>
        {
            entity.HasKey(e => e.InsuranceId).HasName("PK__Insuranc__74231A246AD088CD");

            entity
                .ToTable("Insurance")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("Insurance_Provider_History", "dbo");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.Property(e => e.Payer).HasMaxLength(120);
            entity.Property(e => e.PolicyNumber).HasMaxLength(50);

            entity.HasOne(d => d.Patient).WithMany(p => p.Insurances)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Insurance__Patie__412EB0B6");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patient__970EC3661BDE2EC8");

            entity
                .ToTable("Patient")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("Patient_History", "dbo");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.Mrn, "UQ__Patient__C790FDB4B2397C9C").IsUnique();

            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Mrn)
                .HasMaxLength(20)
                .HasColumnName("MRN");
        });

        modelBuilder.Entity<Procedure>(entity =>
        {
            entity.HasKey(e => e.ProcedureId).HasName("PK__Procedur__54C2E52DDE0C19CF");

            entity.ToTable("Procedure");

            entity.Property(e => e.CptCode).HasMaxLength(10);
            entity.Property(e => e.Description).HasMaxLength(200);

            entity.HasOne(d => d.Encounter).WithMany(p => p.Procedures)
                .HasForeignKey(d => d.EncounterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Procedure__Encou__4D94879B");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("PK__Provider__B54C687D40EF2E4B");

            entity.ToTable("Provider");

            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Specialty).HasMaxLength(100);

            entity.HasOne(d => d.Department).WithMany(p => p.Providers)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Provider__Depart__398D8EEE");
        });

        modelBuilder.Entity<VwAnalyticsDeId>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Analytics_DeId");

            entity.Property(e => e.AgeBand)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.EncounterType).HasMaxLength(30);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<VwClinical>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Clinical");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.EncounterType).HasMaxLength(30);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.IcdCode).HasMaxLength(10);
            entity.Property(e => e.Mrn)
                .HasMaxLength(20)
                .HasColumnName("MRN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
