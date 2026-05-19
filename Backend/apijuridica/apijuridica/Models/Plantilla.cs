using System;
using System.Collections.Generic;

namespace apijuridica.Models;

public partial class Plantilla
{
    public int IdPlantilla { get; set; }

    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public string ContenidoBase { get; set; } = null!;

    public int? Version { get; set; }

    public int CreadoPor { get; set; }

    public virtual Usuario CreadoPorNavigation { get; set; } = null!;

    public virtual ICollection<DocumentosJuridico> DocumentosJuridicos { get; set; } = new List<DocumentosJuridico>();

    public virtual CategoriasPlantilla IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<PlantillaVariable> PlantillaVariables { get; set; } = new List<PlantillaVariable>();
}
