using CharacterHub.Models;

namespace CharacterHub.Services;

public interface IPersonajeService
{
    Task<List<Personaje>> GetAllAsync();
    Task<Personaje> GetByIdAsync(int id);
    Task<List<Personaje>> GetByVideojuegoAsync(string videojuego);
    Task<List<Personaje>> GetByNivelMinimoAsync(int nivel);
    Task<Personaje> CreateAsync(Personaje personaje);
    Task UpdateAsync(int id, Personaje personaje);
    Task DeleteAsync(int id);
}
