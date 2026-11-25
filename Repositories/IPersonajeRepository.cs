using CharacterHub.Models;

namespace CharacterHub.Repositories;

public interface IPersonajeRepository
{
    Task<List<Personaje>> GetAllAsync();
    Task<Personaje?> GetByIdAsync(int id);
    Task<List<Personaje>> GetByVideojuegoAsync(string videojuego);
    Task<List<Personaje>> GetByNivelMinimoAsync(int nivel);
    Task<Personaje> CreateAsync(Personaje personaje);
    Task<bool> UpdateAsync(Personaje personaje);
    Task<bool> DeleteAsync(int id);
}
