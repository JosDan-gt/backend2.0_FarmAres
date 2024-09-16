using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using GranjaLosAres_API.Models;
using GranjaLosAres_API.Data;
using System.Security.Cryptography;

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
            var refreshToken = GenerateRefreshToken();
            await SaveRefreshToken(user, refreshToken);

            return Ok(new { accessToken, refreshToken });

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest model)
        {
            var user = await _context.Usuarios
                .Include(u => u.RefreshTokens)  // Asegúrate de incluir los tokens de actualización
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == model.RefreshToken && !rt.IsRevoked));

            if (user == null)
            {
                return Unauthorized("Usuario no encontrado o token de actualización no válido.");
            }

            var validRefreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == model.RefreshToken);

            if (validRefreshToken == null)
            {
                return Unauthorized("Refresh token no válido.");
            }

            if (validRefreshToken.Expiration < DateTime.Now)
            {
                return Unauthorized("Refresh token expirado.");
            }

            // Generar un nuevo token de acceso
            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            validRefreshToken.IsRevoked = true;  // Invalidar el token anterior
            await SaveRefreshToken(user, newRefreshToken);

            return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
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

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private async Task SaveRefreshToken(Usuario user, string refreshToken)
        {
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Expiration = DateTime.Now.AddDays(double.Parse(_configuration["Jwt:RefreshTokenExpirationDays"])),
                UserId = user.Id,
                IsRevoked = false
            };

            user.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class RefreshTokenRequest
        {
            public string RefreshToken { get; set; }
        }
    }
}
