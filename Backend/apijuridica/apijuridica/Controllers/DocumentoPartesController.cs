using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apijuridica.Models;

namespace apijuridica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentoPartesController : ControllerBase
    {
        private readonly GestiondedocumentosContext _context;

        public DocumentoPartesController(GestiondedocumentosContext context)
        {
            _context = context;
        }

        [HttpGet("Documento/{documentoId}")]
        public async Task<ActionResult<IEnumerable<DocumentoParte>>> GetPartesPorDocumento(int documentoId)
        {
            return await _context.DocumentoPartes
                .Include(dp => dp.IdPersonaNavigation)
                .Where(dp => dp.IdDocumento == documentoId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<DocumentoParte>> PostDocumentoParte(DocumentoParte documentoParte)
        {
            _context.DocumentoPartes.Add(documentoParte);
            await _context.SaveChangesAsync();

            return Ok(documentoParte);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumentoParte(int id)
        {
            var parte = await _context.DocumentoPartes.FindAsync(id);
            if (parte == null) return NotFound();

            _context.DocumentoPartes.Remove(parte);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
