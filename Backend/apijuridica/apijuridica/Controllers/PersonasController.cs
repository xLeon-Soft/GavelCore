using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apijuridica.Models;

namespace apijuridica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonasController : ControllerBase
    {
        private readonly GestiondedocumentosContext _context;

        public PersonasController(GestiondedocumentosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> GetPersonas()
        {
            return await _context.Personas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> GetPersona(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            if (persona == null) return NotFound();
            return persona;
        }

        [HttpPost]
        public async Task<ActionResult<Persona>> PostPersona(Persona persona)
        {
            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPersona", new { id = persona.IdPersona }, persona);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersona(int id, Persona persona)
        {
            if (id != persona.IdPersona) return BadRequest();
            _context.Entry(persona).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
