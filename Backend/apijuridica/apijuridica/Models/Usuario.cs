using System;
using System.Collections.Generic;

namespace apijuridica.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string NombreUsuario { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Dpi { get; set; } = null!;

    public string? Colegiado { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<Auditorium> Auditoria { get; set; } = new List<Auditorium>();

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<DocumentoHistorialEstado> DocumentoHistorialEstados { get; set; } = new List<DocumentoHistorialEstado>();

    public virtual ICollection<DocumentosJuridico> DocumentosJuridicos { get; set; } = new List<DocumentosJuridico>();

    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();

    public virtual ICollection<Plantilla> Plantillas { get; set; } = new List<Plantilla>();
}
