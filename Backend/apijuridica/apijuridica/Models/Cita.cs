using System;
using System.Collections.Generic;

namespace apijuridica.Models;

public partial class Cita
{
    public int IdCita { get; set; }

    public int IdAbogado { get; set; }

    public int? IdCliente { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime FechaHoraInicio { get; set; }

    public DateTime FechaHoraFin { get; set; }

    public string Estado { get; set; } = null!;

    public string? Notas { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Usuario IdAbogadoNavigation { get; set; } = null!;

    public virtual Persona? IdClienteNavigation { get; set; }
}
