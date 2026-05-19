using System;
using System.Collections.Generic;

namespace apijuridica.Models;

public partial class EstadosDocumento
{
    public int IdEstadoDocumento { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<DocumentoHistorialEstado> DocumentoHistorialEstadoIdEstadoAnteriorNavigations { get; set; } = new List<DocumentoHistorialEstado>();

    public virtual ICollection<DocumentoHistorialEstado> DocumentoHistorialEstadoIdEstadoNuevoNavigations { get; set; } = new List<DocumentoHistorialEstado>();

    public virtual ICollection<DocumentosJuridico> DocumentosJuridicos { get; set; } = new List<DocumentosJuridico>();
}
