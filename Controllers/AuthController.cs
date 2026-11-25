using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CharacterHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CharacterHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private const string DemoUser = "admin";
    private const string DemoPass = "CharacterHub2026!";

    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        if (request is null ||
            !string.Equals(request.Username, DemoUser, StringComparison.Ordinal) ||
            !string.Equals(request.Password, DemoPass, StringComparison.Ordinal))
        {
            return Unauthorized(new { error = "Credenciales inválidas." });
        }

        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("No se ha configurado Jwt:Key en User Secrets o variables de entorno.");

        var issuer = _configuration["Jwt:Issuer"] ?? "CharacterHubApi";
        var audience = _configuration["Jwt:Audience"] ?? "CharacterHubClient";
        var expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var mins) ? mins : 60;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, request.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return Ok(new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            Username = request.Username
        });
    }
}
