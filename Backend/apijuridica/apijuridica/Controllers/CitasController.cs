using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apijuridica.Models;

namespace apijuridica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CitasController : ControllerBase
    {
        private readonly GestiondedocumentosContext _context;

        public CitasController(GestiondedocumentosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cita>>> GetCitas()
        {
            return await _context.Citas
                .Include(c => c.IdAbogadoNavigation)
                .Include(c => c.IdClienteNavigation)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetCita(int id)
        {
            var cita = await _context.Citas
                .Include(c => c.IdAbogadoNavigation)
                .Include(c => c.IdClienteNavigation)
                .FirstOrDefaultAsync(c => c.IdCita == id);

            if (cita == null) return NotFound();
            return cita;
        }

        [HttpPost]
        public async Task<ActionResult<Cita>> PostCita(Cita cita)
        {
            cita.FechaCreacion = DateTime.Now;
            cita.FechaModificacion = DateTime.Now;
            _context.Citas.Add(cita);
            
            await _context.Auditoria.AddAsync(new Auditorium
            {
                TablaAfectada = "citas",
                Accion = "CREATE",
                ValorNuevo = $"Cita: {cita.Titulo}",
                FechaEvento = DateTime.Now,
                IdUsuario = int.Parse(User.FindFirst("UserId")?.Value ?? "1")
            });

            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCita", new { id = cita.IdCita }, cita);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCita(int id, Cita cita)
        {
            if (id != cita.IdCita) return BadRequest();

            cita.FechaModificacion = DateTime.Now;
            _context.Entry(cita).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Citas.Any(e => e.IdCita == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return NotFound();

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
