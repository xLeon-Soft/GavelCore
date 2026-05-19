using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apijuridica.Models;
using BCrypt.Net;

namespace apijuridica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMINISTRADOR")]
    public class AdminController : ControllerBase
    {
        private readonly GestiondedocumentosContext _context;

        public AdminController(GestiondedocumentosContext context)
        {
            _context = context;
        }

        [HttpGet("Usuarios")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.Include(u => u.IdRolNavigation).ToListAsync();
        }

        [HttpPost("Usuarios")]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);
            usuario.FechaCreacion = DateTime.Now;
            usuario.Estado = "ACTIVO";

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var persona = new Persona
            {
                NombreCompleto = $"{usuario.Nombres} {usuario.Apellidos}",
                NumeroIdentificacion = usuario.Dpi,
                Correo = usuario.Correo,
                TipoPersona = "INDIVIDUAL",
                Estado = "ACTIVO",
                IdUsuario = usuario.IdUsuario
            };

            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario y Persona creados con éxito", userId = usuario.IdUsuario });
        }

        [HttpGet("Roles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        [HttpGet("Auditoria")]
        public async Task<ActionResult<IEnumerable<Auditorium>>> GetAuditoria()
        {
            return await _context.Auditoria
                .Include(a => a.IdUsuarioNavigation)
                .OrderByDescending(a => a.FechaEvento)
                .Take(100)
                .ToListAsync();
        }
    }
}
