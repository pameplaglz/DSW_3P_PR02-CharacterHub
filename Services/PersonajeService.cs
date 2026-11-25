using CharacterHub.Models;
using CharacterHub.Repositories;

namespace CharacterHub.Services;

public class PersonajeService : IPersonajeService
{
    private readonly IPersonajeRepository _repository;

    public PersonajeService(IPersonajeRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Personaje>> GetAllAsync() => _repository.GetAllAsync();

    public async Task<Personaje> GetByIdAsync(int id)
    {
        var personaje = await _repository.GetByIdAsync(id);
        if (personaje is null)
        {
            throw new KeyNotFoundException($"No se encontró el personaje con Id {id}.");
        }

        return personaje;
    }

    public Task<List<Personaje>> GetByVideojuegoAsync(string videojuego)
    {
        if (string.IsNullOrWhiteSpace(videojuego))
        {
            throw new ArgumentException("El nombre del videojuego es obligatorio.");
        }

        return _repository.GetByVideojuegoAsync(videojuego.Trim());
    }

    public Task<List<Personaje>> GetByNivelMinimoAsync(int nivel)
    {
        if (nivel < 1 || nivel > 100)
        {
            throw new ArgumentException("El nivel debe estar entre 1 y 100.");
        }

        return _repository.GetByNivelMinimoAsync(nivel);
    }

    public Task<Personaje> CreateAsync(Personaje personaje)
    {
        var validado = ValidarYNormalizar(personaje);
        validado.Id = 0;
        validado.Disponible = true;
        return _repository.CreateAsync(validado);
    }

    public async Task UpdateAsync(int id, Personaje personaje)
    {
        var validado = ValidarYNormalizar(personaje);
        validado.Id = id;

        var updated = await _repository.UpdateAsync(validado);
        if (!updated)
        {
            throw new KeyNotFoundException($"No se encontró el personaje con Id {id}.");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
        {
            throw new KeyNotFoundException($"No se encontró el personaje con Id {id}.");
        }
    }

    private static Personaje ValidarYNormalizar(Personaje? personaje)
    {
        if (personaje is null)
        {
            throw new ArgumentException("Los datos del personaje son obligatorios.");
        }

        if (string.IsNullOrWhiteSpace(personaje.Nombre))
        {
            throw new ArgumentException("El nombre del personaje es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(personaje.Videojuego))
        {
            throw new ArgumentException("El videojuego es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(personaje.Clase))
        {
            throw new ArgumentException("La clase del personaje es obligatoria.");
        }

        if (personaje.Nivel < 1 || personaje.Nivel > 100)
        {
            throw new ArgumentException("El nivel debe estar entre 1 y 100.");
        }

        if (personaje.Poder <= 0)
        {
            throw new ArgumentException("El poder debe ser mayor que cero.");
        }

        var imagenUrl = personaje.ImagenUrl?.Trim() ?? string.Empty;
        if (!Uri.TryCreate(imagenUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("ImagenUrl debe ser una URL absoluta válida con esquema HTTP o HTTPS.");
        }

        return new Personaje
        {
            Id = personaje.Id,
            Nombre = personaje.Nombre.Trim(),
            Videojuego = personaje.Videojuego.Trim(),
            Clase = personaje.Clase.Trim(),
            Nivel = personaje.Nivel,
            Poder = personaje.Poder,
            Disponible = personaje.Disponible,
            ImagenUrl = imagenUrl
        };
    }
}
