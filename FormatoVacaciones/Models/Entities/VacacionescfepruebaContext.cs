using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace FormatoVacaciones.Models.Entities;

public partial class VacacionescfepruebaContext : DbContext
{
    public VacacionescfepruebaContext()
    {
    }

    public VacacionescfepruebaContext(DbContextOptions<VacacionescfepruebaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departamento> Departamento { get; set; }

    public virtual DbSet<EstadoSolicitud> EstadoSolicitud { get; set; }

    public virtual DbSet<Puesto> Puesto { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Solicitudvacacion> Solicitudvacacion { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    public virtual DbSet<Vacaciones> Vacaciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=vacacionescfeprueba;user=root;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.43-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.IdDepartamento).HasName("PRIMARY");

            entity.ToTable("departamento");

            entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");
            entity.Property(e => e.NombreDepartamento).HasMaxLength(45);
        });

        modelBuilder.Entity<EstadoSolicitud>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PRIMARY");

            entity.ToTable("estado_solicitud");

            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.NombreEstado).HasMaxLength(50);
        });

        modelBuilder.Entity<Puesto>(entity =>
        {
            entity.HasKey(e => e.IdPuesto).HasName("PRIMARY");

            entity.ToTable("puesto");

            entity.Property(e => e.IdPuesto).HasColumnName("idPuesto");
            entity.Property(e => e.NombrePuesto).HasMaxLength(45);
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.NombreRol).HasMaxLength(45);
        });

        modelBuilder.Entity<Solicitudvacacion>(entity =>
        {
            entity.HasKey(e => e.IdSolicitudVacacion).HasName("PRIMARY");

            entity.ToTable("solicitudvacacion");

            entity.HasIndex(e => e.IdEstado, "IdEstado");

            entity.HasIndex(e => e.IdUsuario, "IdUsuario");

            entity.Property(e => e.IdSolicitudVacacion).HasColumnName("idSolicitudVacacion");
            entity.Property(e => e.Año)
                .HasMaxLength(45)
                .HasColumnName("año");
            entity.Property(e => e.Comentarios).HasColumnType("text");
            entity.Property(e => e.IdEstado).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Solicitudvacacion)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("solicitudvacacion_ibfk_2");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Solicitudvacacion)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("solicitudvacacion_ibfk_1");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.IdjefeDirecto, "IDJefeDirecto");

            entity.HasIndex(e => e.IdDepartamento, "IdDepartamento");

            entity.HasIndex(e => e.IdPuesto, "IdPuesto");

            entity.HasIndex(e => e.IdRol, "IdRol");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.IdjefeDirecto).HasColumnName("IDJefeDirecto");
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.RpRt)
                .HasMaxLength(45)
                .HasColumnName("RP-RT");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_ibfk_3");

            entity.HasOne(d => d.IdPuestoNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdPuesto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_ibfk_4");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_ibfk_2");

            entity.HasOne(d => d.IdjefeDirectoNavigation).WithMany(p => p.InverseIdjefeDirectoNavigation)
                .HasForeignKey(d => d.IdjefeDirecto)
                .HasConstraintName("usuario_ibfk_1");
        });

        modelBuilder.Entity<Vacaciones>(entity =>
        {
            entity.HasKey(e => e.Idvacacion).HasName("PRIMARY");

            entity.ToTable("vacaciones");

            entity.HasIndex(e => e.IdUsuario, "IdUsuario");

            entity.Property(e => e.Idvacacion).HasColumnName("idvacacion");
            entity.Property(e => e.Periodos).HasDefaultValueSql("'4'");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Vacaciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vacaciones_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
