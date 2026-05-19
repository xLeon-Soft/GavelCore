using System;
using System.Collections.Generic;

namespace apijuridica.Models;

public partial class PlantillaVariable
{
    public int IdPlantillaVariable { get; set; }

    public int IdPlantilla { get; set; }

    public string NombreVariable { get; set; } = null!;

    public string Etiqueta { get; set; } = null!;

    public string TipoDato { get; set; } = null!;

    public string OrigenDato { get; set; } = null!;

    public virtual Plantilla IdPlantillaNavigation { get; set; } = null!;
}
