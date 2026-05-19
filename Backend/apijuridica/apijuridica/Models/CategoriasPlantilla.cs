using System;
using System.Collections.Generic;

namespace apijuridica.Models;

public partial class CategoriasPlantilla
{
    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Plantilla> Plantillas { get; set; } = new List<Plantilla>();
}
