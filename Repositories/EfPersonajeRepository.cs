using CharacterHub.Data;
using CharacterHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CharacterHub.Repositories;

public class EfPersonajeRepository : IPersonajeRepository
{
    private readonly AppDbContext _context;

    public EfPersonajeRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Personaje>> GetAllAsync()
    {
        return _context.Personajes.AsNoTracking().ToListAsync();
    }

    public Task<Personaje?> GetByIdAsync(int id)
    {
        return _context.Personajes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<List<Personaje>> GetByVideojuegoAsync(string videojuego)
    {
        var buscado = videojuego.ToLower();
        return _context.Personajes
            .AsNoTracking()
            .Where(p => p.Videojuego.ToLower() == buscado)
            .ToListAsync();
    }

    public Task<List<Personaje>> GetByNivelMinimoAsync(int nivel)
    {
        return _context.Personajes
            .AsNoTracking()
            .Where(p => p.Nivel >= nivel)
            .ToListAsync();
    }

    public async Task<Personaje> CreateAsync(Personaje personaje)
    {
        _context.Personajes.Add(personaje);
        await _context.SaveChangesAsync();
        return personaje;
    }

    public async Task<bool> UpdateAsync(Personaje personaje)
    {
        var existente = await _context.Personajes.FindAsync(personaje.Id);
        if (existente is null)
        {
            return false;
        }

        existente.Nombre = personaje.Nombre;
        existente.Videojuego = personaje.Videojuego;
        existente.Clase = personaje.Clase;
        existente.Nivel = personaje.Nivel;
        existente.Poder = personaje.Poder;
        existente.Disponible = personaje.Disponible;
        existente.ImagenUrl = personaje.ImagenUrl;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existente = await _context.Personajes.FindAsync(id);
        if (existente is null)
        {
            return false;
        }

        _context.Personajes.Remove(existente);
        await _context.SaveChangesAsync();
        return true;
    }
}
