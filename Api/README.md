# API - Productos (aaw-unidad2)

## Descripción

API RESTful construida con **ASP.NET Core (.NET 10)** que implementa un CRUD completo de productos sobre **SQL Server** utilizando **Entity Framework Core** y el patrón **CQRS** con **MediatR** para separar comandos (escrituras) de consultas (lecturas). Las migraciones se aplican automáticamente al iniciar la aplicación y la documentación interactiva se expone mediante **Swagger UI**.

### Stack

| Tecnología | Uso |
|---|---|
| ASP.NET Core Web API | Framework HTTP |
| Entity Framework Core 10 | ORM |
| SQL Server (LocalDB) | Base de datos |
| MediatR 12 | CQRS (comandos y consultas) |
| Swashbuckle | Swagger UI / documentación |

## Endpoints

Base URL: `https://localhost:<puerto>/api`

| Método | Ruta | Descripción | Respuestas |
|---|---|---|---|
| GET | `/api/Producto` | Lista todos los productos | 200 |
| GET | `/api/Producto/{id}` | Obtiene un producto por Id | 200, 404 |
| POST | `/api/Producto` | Crea un producto | 201, 400 |
| PUT | `/api/Producto/{id}` | Actualiza un producto existente | 204, 404 |
| DELETE | `/api/Producto/{id}` | Elimina un producto | 204, 404 |

Documentación interactiva: `https://localhost:<puerto>/swagger` (solo en desarrollo).

### Ejemplo de payload

```json
{
  "id": 0,
  "nombre": "Teclado",
  "descripcion": "Teclado mecánico RGB",
  "precio": 89.99
}
```

## Conexiones externas

- **SQL Server**: configurada en `appsettings.json` bajo `ConnectionStrings:DefaultConnection`.
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ApiDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
  ```
  Modificar si se usa SQL Server Express u otra instancia.

- **Swagger UI** (`/swagger`): documentación generada a partir de los controladores.

## Estructura del proyecto

```
Api/
├── Controllers/
│   └── ProductoController.cs      # Endpoints REST (delegan en MediatR)
├── Features/
│   └── Productos/
│       ├── Commands/              # CQRS - escrituras
│       │   ├── CreateProductoCommand.cs
│       │   ├── UpdateProductoCommand.cs
│       │   └── DeleteProductoCommand.cs
│       └── Queries/               # CQRS - lecturas
│           ├── GetProductosQuery.cs
│           └── GetProductoByIdQuery.cs
├── Models/
│   └── Producto.cs                # Entidad de dominio
├── Data/
│   └── AppDbContext.cs            # DbContext + seed de datos
├── Migrations/                    # Migraciones EF Core
└── Program.cs                     # Configuración y arranque
```

## Diagramas de secuencia

### GET - Listar todos los productos

```mermaid
sequenceDiagram
    actor Cliente
    participant C as ProductoController
    participant M as IMediator
    participant Q as GetProductosQueryHandler
    participant DB as AppDbContext (EF Core)
    participant S as SQL Server

    Cliente->>C: GET /api/Producto
    C->>M: Send(GetProductosQuery)
    M->>Q: Handle(query)
    Q->>DB: Productos.AsNoTracking().ToListAsync()
    DB->>S: SELECT * FROM Productos
    S-->>DB: Filas
    DB-->>Q: List<Producto>
    Q-->>M: List<Producto>
    M-->>C: List<Producto>
    C-->>Cliente: 200 OK [productos]
```

### GET - Obtener producto por Id

```mermaid
sequenceDiagram
    actor Cliente
    participant C as ProductoController
    participant M as IMediator
    participant Q as GetProductoByIdQueryHandler
    participant DB as AppDbContext (EF Core)
    participant S as SQL Server

    Cliente->>C: GET /api/Producto/{id}
    C->>M: Send(GetProductoByIdQuery(id))
    M->>Q: Handle(query)
    Q->>DB: FirstOrDefaultAsync(p => p.Id == id)
    DB->>S: SELECT ... WHERE Id = @id
    S-->>DB: Fila o vacío
    DB-->>Q: Producto? 
    Q-->>M: Producto?
    M-->>C: Producto? 
    alt Producto existe
        C-->>Cliente: 200 OK [producto]
    else No existe
        C-->>Cliente: 404 Not Found
    end
```

### POST - Crear producto

```mermaid
sequenceDiagram
    actor Cliente
    participant C as ProductoController
    participant M as IMediator
    participant Cmd as CreateProductoCommandHandler
    participant DB as AppDbContext (EF Core)
    participant S as SQL Server

    Cliente->>C: POST /api/Producto [body]
    C->>M: Send(CreateProductoCommand)
    M->>Cmd: Handle(command)
    Cmd->>Cmd: Mapear command → Producto
    Cmd->>DB: Productos.Add(producto)
    Cmd->>DB: SaveChangesAsync()
    DB->>S: INSERT INTO Productos (...) OUTPUT Id
    S-->>DB: Id generado
    DB-->>Cmd: Filas afectadas
    Cmd-->>M: Producto (con Id)
    M-->>C: Producto
    C-->>Cliente: 201 Created (Location: /api/Producto/{id})
```

### PUT - Actualizar producto

```mermaid
sequenceDiagram
    actor Cliente
    participant C as ProductoController
    participant M as IMediator
    participant Cmd as UpdateProductoCommandHandler
    participant DB as AppDbContext (EF Core)
    participant S as SQL Server

    Cliente->>C: PUT /api/Producto/{id} [body]
    C->>M: Send(UpdateProductoCommand)
    M->>Cmd: Handle(command)
    Cmd->>DB: FindAsync(id)
    DB->>S: SELECT ... WHERE Id = @id
    S-->>DB: Fila o vacío
    DB-->>Cmd: Producto?
    alt Producto no existe
        Cmd-->>M: false
        M-->>C: false
        C-->>Cliente: 404 Not Found
    else Producto existe
        Cmd->>Cmd: Actualizar Nombre, Descripcion, Precio
        Cmd->>DB: SaveChangesAsync()
        DB->>S: UPDATE Productos SET ...
        S-->>DB: OK
        Cmd-->>M: true
        M-->>C: true
        C-->>Cliente: 204 No Content
    end
```

### DELETE - Eliminar producto

```mermaid
sequenceDiagram
    actor Cliente
    participant C as ProductoController
    participant M as IMediator
    participant Cmd as DeleteProductoCommandHandler
    participant DB as AppDbContext (EF Core)
    participant S as SQL Server

    Cliente->>C: DELETE /api/Producto/{id}
    C->>M: Send(DeleteProductoCommand(id))
    M->>Cmd: Handle(command)
    Cmd->>DB: FindAsync(id)
    DB->>S: SELECT ... WHERE Id = @id
    S-->>DB: Fila o vacío
    DB-->>Cmd: Producto?
    alt Producto no existe
        Cmd-->>M: false
        M-->>C: false
        C-->>Cliente: 404 Not Found
    else Producto existe
        Cmd->>DB: Productos.Remove(producto)
        Cmd->>DB: SaveChangesAsync()
        DB->>S: DELETE FROM Productos WHERE Id = @id
        S-->>DB: OK
        Cmd-->>M: true
        M-->>C: true
        C-->>Cliente: 204 No Content
    end
```

### Arranque de la aplicación

```mermaid
sequenceDiagram
    participant Host as dotnet run
    participant P as Program.cs
    participant DI as Contenedor DI
    participant DB as AppDbContext (EF Core)
    participant S as SQL Server

    Host->>P: Ejecutar app
    P->>DI: Registrar DbContext, MediatR, Swagger
    P->>P: Build() + MapControllers()
    P->>DI: CreateScope()
    DI-->>DB: Resolver AppDbContext
    DB->>S: Aplicar migraciones pendientes (Database.Migrate())
    Note over DB,S: Si la BD no existe, se crea.<br/>Se insertan datos semilla (HasData).
    P->>Host: app.Run() — escucha requests
```

## Cómo ejecutar

```bash
dotnet restore
dotnet run
```

Las migraciones (creación de tablas y datos semilla) se aplican automáticamente al iniciar. Luego abrir `https://localhost:<puerto>/swagger`.
