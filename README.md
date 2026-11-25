# DSW_3P_PR02 – CharacterHub: API de Gestión de Personajes

API REST desarrollada con ASP.NET Core 10 para administrar personajes de videojuegos, utilizando arquitectura en capas (`Controller` → `Service` → `Repository`), Entity Framework Core con SQL Server, autenticación JWT, CORS y Swagger UI.

## Requisitos previos

- SDK de .NET 10
- SQL Server (`localhost\SQLEXPRESS` configurado en `appsettings.json`)

## Configuración inicial (clave JWT fuera del repositorio)

La clave de firma JWT no se guarda en el código fuente ni en `appsettings.json`. Para configurarla localmente antes de ejecutar la API:

```sh
dotnet user-secrets set "Jwt:Key" "TuClaveSecretaDeAlMenos32Caracteres!"
```

Para aplicar la migración a SQL Server:

```sh
dotnet ef database update
```

## Ejecutar la API

```sh
dotnet run
```

- URL base: `http://localhost:5050`
- Swagger UI: `http://localhost:5050/swagger`

## Usuario de demostración (JWT)

- **Endpoint:** `POST /api/auth/login`
- **Usuario:** `admin`
- **Contraseña:** `CharacterHub2026!`

## CORS

Los orígenes permitidos se configuran en `appsettings.json` bajo `Cors:AllowedOrigins`. El día de la evaluación puedes agregar ahí la URL exacta del cliente proporcionado por el docente y reiniciar la API.

## Endpoints principales

| Método | Ruta | Autenticación | Descripción |
| --- | --- | --- | --- |
| POST | `/api/auth/login` | Pública | Inicia sesión y devuelve un token JWT |
| GET | `/api/personajes` | JWT | Lista todos los personajes |
| GET | `/api/personajes/{id}` | JWT | Consulta un personaje por ID |
| GET | `/api/personajes/videojuego/{nombre}` | JWT | Filtra personajes por videojuego |
| GET | `/api/personajes/nivel/{nivel}` | JWT | Filtra personajes con nivel ≥ al indicado |
| POST | `/api/personajes` | JWT | Registra un nuevo personaje (`Disponible = true`) |
| PUT | `/api/personajes/{id}` | JWT | Actualiza un personaje existente |
| DELETE | `/api/personajes/{id}` | JWT | Elimina un personaje |
