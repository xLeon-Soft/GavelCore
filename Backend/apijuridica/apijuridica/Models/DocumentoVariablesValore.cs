using System;
using System.Collections.Generic;

namespace apijuridica.Models;

public partial class DocumentoVariablesValore
{
    public int IdDocumentoVariable { get; set; }

    public int IdDocumento { get; set; }

    public string NombreVariable { get; set; } = null!;

    public string? ValorVariable { get; set; }

    public virtual DocumentosJuridico IdDocumentoNavigation { get; set; } = null!;
}
