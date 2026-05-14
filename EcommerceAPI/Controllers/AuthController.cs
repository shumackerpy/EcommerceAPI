using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;


namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
         
        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
         
        // REGISTRO 
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
         
        // LOGIN
        [HttpPost("login")]
        public IActionResult Login(User login)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Correo == login.Correo &&
                u.Contrasena == login.Contrasena);

            if (user == null)
                return Unauthorized();

            var token = GenerateToken(user);

            return Ok(new { token });
        }

        // GENERAR TOKEN
        private string GenerateToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.Email, user.Correo),
                new Claim(ClaimTypes.Role, user.Rol)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );
        
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

 
  