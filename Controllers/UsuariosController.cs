using GranjaLosAres_API.Data;
using GranjaLosAres_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GranjaLosAres_API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MyDbContext _context;

        public UsuariosController(IConfiguration configuration, MyDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Usuarios
                                      .Include(u => u.Role)  // Incluye la relación con los roles
                                      .ToListAsync();

            if (users == null || users.Count == 0)
            {
                return NotFound("No se encontraron usuarios.");
            }

            // Mapeo a un DTO si no deseas devolver la entidad completa (opcional)
            var usersDto = users.Select(user => new
            {
                user.Id,
                user.NombreUser,
                user.Email,
                user.Estado,
                Role = user.Role?.Nombre ?? "SinRol"  // Devuelve el nombre del rol o "SinRol" si no tiene rol
            });

            return Ok(usersDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            if (roles == null || roles.Count == 0)
            {
                return NotFound("No se encontraron roles.");
            }

            return Ok(roles);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario usuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.NombreUser == usuario.NombreUser || u.Email == usuario.Email))
            {
                return BadRequest("El nombre de usuario o el correo electrónico ya están en uso.");
            }

            usuario.FechaDeRegistro = DateTime.Now;
            usuario.Estado = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario registrado exitosamente" });
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Usuario updatedUser)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Actualizar los campos del usuario
            user.NombreUser = updatedUser.NombreUser;
            user.Email = updatedUser.Email;

            // Solo actualizamos la contraseña si se proporciona
            if (!string.IsNullOrWhiteSpace(updatedUser.Contrasena))
            {
                user.Contrasena = updatedUser.Contrasena;
            }

            // Mantener el estado siempre en activo (true)
            user.Estado = true;

            user.RoleId = updatedUser.RoleId;

            _context.Usuarios.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario actualizado exitosamente" });
        }




        [Authorize(Roles = "Admin")]
        [HttpPut("disable/{id}")]
        public async Task<IActionResult> DisableUser(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Deshabilitación lógica
            user.Estado = false;  // Cambiar el estado a falso para deshabilitar al usuario

            _context.Usuarios.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario deshabilitado exitosamente" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("enable/{id}")]
        public async Task<IActionResult> EnableUser(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Habilitar el usuario
            user.Estado = true;  // Cambiar el estado a true para habilitar al usuario

            _context.Usuarios.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario habilitado exitosamente" });
        }
    }
}
