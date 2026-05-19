using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace apijuridica.Models;

public partial class GestiondedocumentosContext : DbContext
{
    public GestiondedocumentosContext()
    {
    }

    public GestiondedocumentosContext(DbContextOptions<GestiondedocumentosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<CategoriasPlantilla> CategoriasPlantillas { get; set; }

    public virtual DbSet<Cita> Citas { get; set; }

    public virtual DbSet<DocumentoHistorialEstado> DocumentoHistorialEstados { get; set; }

    public virtual DbSet<DocumentoParte> DocumentoPartes { get; set; }

    public virtual DbSet<DocumentoVariablesValore> DocumentoVariablesValores { get; set; }

    public virtual DbSet<DocumentosJuridico> DocumentosJuridicos { get; set; }

    public virtual DbSet<EstadosDocumento> EstadosDocumentos { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Plantilla> Plantillas { get; set; }

    public virtual DbSet<PlantillaVariable> PlantillaVariables { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria);

            entity.ToTable("auditoria");

            entity.HasIndex(e => e.FechaEvento, "IX_auditoria_fecha");

            entity.Property(e => e.IdAuditoria).HasColumnName("id_auditoria");
            entity.Property(e => e.Accion)
                .HasMaxLength(50)
                .HasColumnName("accion");
            entity.Property(e => e.FechaEvento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_evento");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.TablaAfectada)
                .HasMaxLength(100)
                .HasColumnName("tabla_afectada");
            entity.Property(e => e.ValorAnterior).HasColumnName("valor_anterior");
            entity.Property(e => e.ValorNuevo).HasColumnName("valor_nuevo");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_auditoria_usuarios");
        });

        modelBuilder.Entity<CategoriasPlantilla>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.ToTable("categorias_plantilla");

            entity.HasIndex(e => e.Nombre, "UK_categorias_plantilla_nombre").IsUnique();

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.IdCita);

            entity.ToTable("citas");

            entity.HasIndex(e => e.FechaHoraInicio, "IX_citas_fecha_inicio");

            entity.Property(e => e.IdCita).HasColumnName("id_cita");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("PENDIENTE")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaHoraFin)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora_fin");
            entity.Property(e => e.FechaHoraInicio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora_inicio");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.IdAbogado).HasColumnName("id_abogado");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.Titulo)
                .HasMaxLength(200)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdAbogadoNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdAbogado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_citas_abogado");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_citas_cliente");
        });

        modelBuilder.Entity<DocumentoHistorialEstado>(entity =>
        {
            entity.HasKey(e => e.IdHistorial);

            entity.ToTable("documento_historial_estados");

            entity.Property(e => e.IdHistorial).HasColumnName("id_historial");
            entity.Property(e => e.CambiadoPor).HasColumnName("cambiado_por");
            entity.Property(e => e.Comentario)
                .HasMaxLength(500)
                .HasColumnName("comentario");
            entity.Property(e => e.FechaCambio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_cambio");
            entity.Property(e => e.IdDocumento).HasColumnName("id_documento");
            entity.Property(e => e.IdEstadoAnterior).HasColumnName("id_estado_anterior");
            entity.Property(e => e.IdEstadoNuevo).HasColumnName("id_estado_nuevo");

            entity.HasOne(d => d.CambiadoPorNavigation).WithMany(p => p.DocumentoHistorialEstados)
                .HasForeignKey(d => d.CambiadoPor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_historial_usuarios");

            entity.HasOne(d => d.IdDocumentoNavigation).WithMany(p => p.DocumentoHistorialEstados)
                .HasForeignKey(d => d.IdDocumento)
                .HasConstraintName("FK_historial_documentos");

            entity.HasOne(d => d.IdEstadoAnteriorNavigation).WithMany(p => p.DocumentoHistorialEstadoIdEstadoAnteriorNavigations)
                .HasForeignKey(d => d.IdEstadoAnterior)
                .HasConstraintName("FK_historial_estado_anterior");

            entity.HasOne(d => d.IdEstadoNuevoNavigation).WithMany(p => p.DocumentoHistorialEstadoIdEstadoNuevoNavigations)
                .HasForeignKey(d => d.IdEstadoNuevo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_historial_estado_nuevo");
        });

        modelBuilder.Entity<DocumentoParte>(entity =>
        {
            entity.HasKey(e => e.IdDocumentoParte);

            entity.ToTable("documento_partes");

            entity.Property(e => e.IdDocumentoParte).HasColumnName("id_documento_parte");
            entity.Property(e => e.IdDocumento).HasColumnName("id_documento");
            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.RolEnDocumento)
                .HasMaxLength(50)
                .HasColumnName("rol_en_documento");

            entity.HasOne(d => d.IdDocumentoNavigation).WithMany(p => p.DocumentoPartes)
                .HasForeignKey(d => d.IdDocumento)
                .HasConstraintName("FK_documento_partes_documentos");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.DocumentoPartes)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_documento_partes_personas");
        });

        modelBuilder.Entity<DocumentoVariablesValore>(entity =>
        {
            entity.HasKey(e => e.IdDocumentoVariable);

            entity.ToTable("documento_variables_valores");

            entity.Property(e => e.IdDocumentoVariable).HasColumnName("id_documento_variable");
            entity.Property(e => e.IdDocumento).HasColumnName("id_documento");
            entity.Property(e => e.NombreVariable)
                .HasMaxLength(100)
                .HasColumnName("nombre_variable");
            entity.Property(e => e.ValorVariable).HasColumnName("valor_variable");

            entity.HasOne(d => d.IdDocumentoNavigation).WithMany(p => p.DocumentoVariablesValores)
                .HasForeignKey(d => d.IdDocumento)
                .HasConstraintName("FK_docvarval_documentos");
        });

        modelBuilder.Entity<DocumentosJuridico>(entity =>
        {
            entity.HasKey(e => e.IdDocumento);

            entity.ToTable("documentos_juridicos");

            entity.HasIndex(e => e.NumeroDocumento, "UK_documentos_numero").IsUnique();

            entity.Property(e => e.IdDocumento).HasColumnName("id_documento");
            entity.Property(e => e.ContenidoGenerado).HasColumnName("contenido_generado");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.IdEstadoDocumento).HasColumnName("id_estado_documento");
            entity.Property(e => e.IdPlantilla).HasColumnName("id_plantilla");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(50)
                .HasColumnName("numero_documento");
            entity.Property(e => e.RutaPdf)
                .HasMaxLength(500)
                .HasColumnName("ruta_pdf");
            entity.Property(e => e.Titulo)
                .HasMaxLength(200)
                .HasColumnName("titulo");

            entity.HasOne(d => d.CreadoPorNavigation).WithMany(p => p.DocumentosJuridicos)
                .HasForeignKey(d => d.CreadoPor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_documentos_usuarios");

            entity.HasOne(d => d.IdEstadoDocumentoNavigation).WithMany(p => p.DocumentosJuridicos)
                .HasForeignKey(d => d.IdEstadoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_documentos_estados");

            entity.HasOne(d => d.IdPlantillaNavigation).WithMany(p => p.DocumentosJuridicos)
                .HasForeignKey(d => d.IdPlantilla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_documentos_plantillas");
        });

        modelBuilder.Entity<EstadosDocumento>(entity =>
        {
            entity.HasKey(e => e.IdEstadoDocumento);

            entity.ToTable("estados_documento");

            entity.HasIndex(e => e.Nombre, "UK_estados_documento_nombre").IsUnique();

            entity.Property(e => e.IdEstadoDocumento).HasColumnName("id_estado_documento");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona);

            entity.ToTable("personas");

            entity.HasIndex(e => e.NumeroIdentificacion, "IX_personas_identificacion");

            entity.HasIndex(e => e.NombreCompleto, "IX_personas_nombre");

            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .HasColumnName("correo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(500)
                .HasColumnName("direccion");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("ACTIVO")
                .HasColumnName("estado");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Nit)
                .HasMaxLength(30)
                .HasColumnName("nit");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(255)
                .HasColumnName("nombre_completo");
            entity.Property(e => e.NumeroIdentificacion)
                .HasMaxLength(50)
                .HasColumnName("numero_identificacion");
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .HasColumnName("telefono");
            entity.Property(e => e.TipoPersona)
                .HasMaxLength(20)
                .HasDefaultValue("INDIVIDUAL")
                .HasColumnName("tipo_persona");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_personas_usuarios");
        });

        modelBuilder.Entity<Plantilla>(entity =>
        {
            entity.HasKey(e => e.IdPlantilla);

            entity.ToTable("plantillas");

            entity.Property(e => e.IdPlantilla).HasColumnName("id_plantilla");
            entity.Property(e => e.ContenidoBase).HasColumnName("contenido_base");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .HasColumnName("nombre");
            entity.Property(e => e.Version)
                .HasDefaultValue(1)
                .HasColumnName("version");

            entity.HasOne(d => d.CreadoPorNavigation).WithMany(p => p.Plantillas)
                .HasForeignKey(d => d.CreadoPor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_plantillas_usuarios");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Plantillas)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_plantillas_categorias");
        });

        modelBuilder.Entity<PlantillaVariable>(entity =>
        {
            entity.HasKey(e => e.IdPlantillaVariable);

            entity.ToTable("plantilla_variables");

            entity.Property(e => e.IdPlantillaVariable).HasColumnName("id_plantilla_variable");
            entity.Property(e => e.Etiqueta)
                .HasMaxLength(150)
                .HasColumnName("etiqueta");
            entity.Property(e => e.IdPlantilla).HasColumnName("id_plantilla");
            entity.Property(e => e.NombreVariable)
                .HasMaxLength(100)
                .HasColumnName("nombre_variable");
            entity.Property(e => e.OrigenDato)
                .HasMaxLength(30)
                .HasDefaultValue("formulario")
                .HasColumnName("origen_dato");
            entity.Property(e => e.TipoDato)
                .HasMaxLength(30)
                .HasDefaultValue("texto")
                .HasColumnName("tipo_dato");

            entity.HasOne(d => d.IdPlantillaNavigation).WithMany(p => p.PlantillaVariables)
                .HasForeignKey(d => d.IdPlantilla)
                .HasConstraintName("FK_plantilla_variables_plantillas");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("roles");

            entity.HasIndex(e => e.Nombre, "UK_roles_nombre").IsUnique();

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Colegiado, "UK_usuarios_colegiado").IsUnique();

            entity.HasIndex(e => e.Correo, "UK_usuarios_correo").IsUnique();

            entity.HasIndex(e => e.Dpi, "UK_usuarios_dpi").IsUnique();

            entity.HasIndex(e => e.NombreUsuario, "UK_usuarios_nombre_usuario").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(150)
                .HasColumnName("apellidos");
            entity.Property(e => e.Colegiado)
                .HasMaxLength(20)
                .HasColumnName("colegiado");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .HasColumnName("correo");
            entity.Property(e => e.Dpi)
                .HasMaxLength(13)
                .HasColumnName("dpi");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("ACTIVO")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.Nombres)
                .HasMaxLength(150)
                .HasColumnName("nombres");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuarios_roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
