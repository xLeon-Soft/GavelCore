using System;
using System.Collections.Generic;

namespace apijuridica.Models;

public partial class DocumentosJuridico
{
    public int IdDocumento { get; set; }

    public string NumeroDocumento { get; set; } = null!;

    public int IdPlantilla { get; set; }

    public int IdEstadoDocumento { get; set; }

    public string Titulo { get; set; } = null!;

    public string ContenidoGenerado { get; set; } = null!;

    public string? RutaPdf { get; set; }

    public int CreadoPor { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Usuario CreadoPorNavigation { get; set; } = null!;

    public virtual ICollection<DocumentoHistorialEstado> DocumentoHistorialEstados { get; set; } = new List<DocumentoHistorialEstado>();

    public virtual ICollection<DocumentoParte> DocumentoPartes { get; set; } = new List<DocumentoParte>();

    public virtual ICollection<DocumentoVariablesValore> DocumentoVariablesValores { get; set; } = new List<DocumentoVariablesValore>();

    public virtual EstadosDocumento IdEstadoDocumentoNavigation { get; set; } = null!;

    public virtual Plantilla IdPlantillaNavigation { get; set; } = null!;
}
