using CharacterHub.Models;
using CharacterHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace CharacterHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonajesController : ControllerBase
{
    private readonly IPersonajeService _service;

    public PersonajesController(IPersonajeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Personaje>>> GetAll()
    {
        var personajes = await _service.GetAllAsync();
        return Ok(personajes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Personaje>> GetById(int id)
    {
        var personaje = await _service.GetByIdAsync(id);
        return Ok(personaje);
    }

    [HttpGet("videojuego/{nombre}")]
    public async Task<ActionResult<List<Personaje>>> GetByVideojuego(string nombre)
    {
        var personajes = await _service.GetByVideojuegoAsync(nombre);
        return Ok(personajes);
    }

    [HttpGet("nivel/{nivel:int}")]
    public async Task<ActionResult<List<Personaje>>> GetByNivel(int nivel)
    {
        var personajes = await _service.GetByNivelMinimoAsync(nivel);
        return Ok(personajes);
    }

    [HttpPost]
    public async Task<ActionResult<Personaje>> Create([FromBody] Personaje personaje)
    {
        var creado = await _service.CreateAsync(personaje);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Personaje personaje)
    {
        await _service.UpdateAsync(id, personaje);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
