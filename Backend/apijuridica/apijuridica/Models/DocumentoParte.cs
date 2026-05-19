using System;
using System.Collections.Generic;

namespace apijuridica.Models;

public partial class DocumentoParte
{
    public int IdDocumentoParte { get; set; }

    public int IdDocumento { get; set; }

    public int IdPersona { get; set; }

    public string RolEnDocumento { get; set; } = null!;

    public virtual DocumentosJuridico IdDocumentoNavigation { get; set; } = null!;

    public virtual Persona IdPersonaNavigation { get; set; } = null!;
}
