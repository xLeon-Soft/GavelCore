using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apijuridica.Models;

namespace apijuridica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlantillasController : ControllerBase
    {
        private readonly GestiondedocumentosContext _context;

        public PlantillasController(GestiondedocumentosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plantilla>>> GetPlantillas()
        {
            return await _context.Plantillas
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.PlantillaVariables)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plantilla>> GetPlantilla(int id)
        {
            var plantilla = await _context.Plantillas
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.PlantillaVariables)
                .FirstOrDefaultAsync(p => p.IdPlantilla == id);

            if (plantilla == null) return NotFound();
            return plantilla;
        }

        [HttpPost]
        public async Task<ActionResult<Plantilla>> PostPlantilla(Plantilla plantilla)
        {
            _context.Plantillas.Add(plantilla);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlantilla", new { id = plantilla.IdPlantilla }, plantilla);
        }

        [HttpGet("Categorias")]
        public async Task<ActionResult<IEnumerable<CategoriasPlantilla>>> GetCategorias()
        {
            return await _context.CategoriasPlantillas.ToListAsync();
        }
    }
}
