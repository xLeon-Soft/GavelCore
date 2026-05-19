using System;
using System.Collections.Generic;

namespace apijuridica.Models;

public partial class DocumentoHistorialEstado
{
    public int IdHistorial { get; set; }

    public int IdDocumento { get; set; }

    public int? IdEstadoAnterior { get; set; }

    public int IdEstadoNuevo { get; set; }

    public int CambiadoPor { get; set; }

    public DateTime? FechaCambio { get; set; }

    public string? Comentario { get; set; }

    public virtual Usuario CambiadoPorNavigation { get; set; } = null!;

    public virtual DocumentosJuridico IdDocumentoNavigation { get; set; } = null!;

    public virtual EstadosDocumento? IdEstadoAnteriorNavigation { get; set; }

    public virtual EstadosDocumento IdEstadoNuevoNavigation { get; set; } = null!;
}
