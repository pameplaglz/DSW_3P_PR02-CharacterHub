using System.Text.Json;
using CharacterHub.Models;

namespace CharacterHub.Repositories;

public class JsonPersonajeRepository : IPersonajeRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private readonly string _filePath;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public JsonPersonajeRepository(IConfiguration configuration, IWebHostEnvironment env)
    {
        var configuredPath = configuration["JsonStorage:FilePath"] ?? "Data/personajes.json";
        _filePath = Path.IsPathRooted(configuredPath)
            ? configuredPath
            : Path.Combine(env.ContentRootPath, configuredPath);
    }

    public async Task<List<Personaje>> GetAllAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            return await ReadAllInternalAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Personaje?> GetByIdAsync(int id)
    {
        await _semaphore.WaitAsync();
        try
        {
            var personajes = await ReadAllInternalAsync();
            return personajes.FirstOrDefault(p => p.Id == id);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<Personaje>> GetByVideojuegoAsync(string videojuego)
    {
        await _semaphore.WaitAsync();
        try
        {
            var personajes = await ReadAllInternalAsync();
            return personajes
                .Where(p => string.Equals(p.Videojuego, videojuego, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<Personaje>> GetByNivelMinimoAsync(int nivel)
    {
        await _semaphore.WaitAsync();
        try
        {
            var personajes = await ReadAllInternalAsync();
            return personajes.Where(p => p.Nivel >= nivel).ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Personaje> CreateAsync(Personaje personaje)
    {
        await _semaphore.WaitAsync();
        try
        {
            var personajes = await ReadAllInternalAsync();
            personaje.Id = personajes.Count > 0 ? personajes.Max(p => p.Id) + 1 : 1;
            personajes.Add(personaje);
            await WriteAllInternalAsync(personajes);
            return personaje;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> UpdateAsync(Personaje personaje)
    {
        await _semaphore.WaitAsync();
        try
        {
            var personajes = await ReadAllInternalAsync();
            var index = personajes.FindIndex(p => p.Id == personaje.Id);
            if (index < 0)
            {
                return false;
            }

            personajes[index] = personaje;
            await WriteAllInternalAsync(personajes);
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _semaphore.WaitAsync();
        try
        {
            var personajes = await ReadAllInternalAsync();
            var removed = personajes.RemoveAll(p => p.Id == id);
            if (removed == 0)
            {
                return false;
            }

            await WriteAllInternalAsync(personajes);
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<List<Personaje>> ReadAllInternalAsync()
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        await using var stream = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<List<Personaje>>(stream, JsonOptions) ?? [];
    }

    private async Task WriteAllInternalAsync(List<Personaje> personajes)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var tempPath = $"{_filePath}.{Guid.NewGuid():N}.tmp";
        await using (var stream = File.Create(tempPath))
        {
            await JsonSerializer.SerializeAsync(stream, personajes, JsonOptions);
        }

        File.Move(tempPath, _filePath, overwrite: true);
    }
}
