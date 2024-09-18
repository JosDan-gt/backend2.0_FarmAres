using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using GranjaLosAres_API.Models;
using GranjaLosAres_API.Data;

namespace GranjaLosAres_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MyDbContext _context;

        public AuthController(IConfiguration configuration, MyDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = await _context.Usuarios
                         .Include(u => u.Role)  // Incluir el rol
                         .SingleOrDefaultAsync(u => u.NombreUser == login.Username);

            if (user == null || login.Password != user.Contrasena)
            {
                return Unauthorized();
            }

            // Depuración: Verificar si el rol está presente
            Console.WriteLine("El rol del usuario es: " + user.Role?.Nombre);

            var accessToken = GenerateJwtToken(user);

            return Ok(new { accessToken });
        }

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

        private string GenerateJwtToken(Usuario user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.NombreUser),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (user.Role != null && !string.IsNullOrEmpty(user.Role.Nombre))
            {
                // Asegurarte de que el rol "Admin" se está incluyendo correctamente
                claims.Add(new Claim(ClaimTypes.Role, user.Role.Nombre));
                Console.WriteLine("Rol asignado en el JWT: " + user.Role.Nombre);
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "SinRol"));
            }

            var expiresInMinutesString = _configuration["Jwt:AccessTokenExpirationMinutes"];
            if (string.IsNullOrEmpty(expiresInMinutesString))
            {
                throw new InvalidOperationException("El valor de Jwt:AccessTokenExpirationMinutes no está configurado.");
            }

            var expiresInMinutes = double.Parse(expiresInMinutesString);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
