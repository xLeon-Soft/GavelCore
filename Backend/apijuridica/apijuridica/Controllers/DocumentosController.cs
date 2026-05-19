using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apijuridica.Models;

namespace apijuridica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentosController : ControllerBase
    {
        private readonly GestiondedocumentosContext _context;

        public DocumentosController(GestiondedocumentosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentosJuridico>>> GetDocumentos()
        {
            return await _context.DocumentosJuridicos
                .Include(d => d.IdPlantillaNavigation)
                .Include(d => d.IdEstadoDocumentoNavigation)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentosJuridico>> GetDocumento(int id)
        {
            var documento = await _context.DocumentosJuridicos
                .Include(d => d.IdPlantillaNavigation)
                .Include(d => d.IdEstadoDocumentoNavigation)
                .FirstOrDefaultAsync(d => d.IdDocumento == id);

            if (documento == null)
            {
                return NotFound();
            }

            return documento;
        }

        [HttpPost]
        public async Task<ActionResult<DocumentosJuridico>> PostDocumento(DocumentosJuridico documento)
        {
            documento.FechaCreacion = DateTime.Now;
            documento.FechaModificacion = DateTime.Now;
            
            _context.DocumentosJuridicos.Add(documento);
            await _context.Set<Auditorium>().AddAsync(new Auditorium
            {
                TablaAfectada = "documentos_juridicos",
                Accion = "CREATE",
                ValorNuevo = documento.Titulo,
                FechaEvento = DateTime.Now,
                IdUsuario = int.Parse(User.FindFirst("UserId")?.Value ?? "1")
            });

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocumento", new { id = documento.IdDocumento }, documento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumento(int id, DocumentosJuridico documento)
        {
            if (id != documento.IdDocumento)
            {
                return BadRequest();
            }

            documento.FechaModificacion = DateTime.Now;
            _context.Entry(documento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumento(int id)
        {
            var documento = await _context.DocumentosJuridicos.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }

            _context.DocumentosJuridicos.Remove(documento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentoExists(int id)
        {
            return _context.DocumentosJuridicos.Any(e => e.IdDocumento == id);
        }
    }
}
