using System;
using System.Collections.Generic;

namespace apijuridica.Models;

public partial class Auditorium
{
    public int IdAuditoria { get; set; }

    public string TablaAfectada { get; set; } = null!;

    public string Accion { get; set; } = null!;

    public string? ValorAnterior { get; set; }

    public string? ValorNuevo { get; set; }

    public DateTime? FechaEvento { get; set; }

    public int IdUsuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
