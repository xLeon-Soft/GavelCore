using System;
using System.Collections.Generic;

namespace apijuridica.Models;

public partial class Persona
{
    public int IdPersona { get; set; }

    public string? TipoPersona { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string? NumeroIdentificacion { get; set; }

    public string? Nit { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public string? Direccion { get; set; }

    public string? Estado { get; set; }

    public int? IdUsuario { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<DocumentoParte> DocumentoPartes { get; set; } = new List<DocumentoParte>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
