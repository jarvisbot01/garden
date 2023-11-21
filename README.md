# Garden API en ASPNet 7.0

## Introducción al Proyecto

Bienvenido al emocionante mundo de Garden API en ASP.Net 7.0. Este proyecto representa una solución completa para la gestión de una tienda de mascotas, abarcando desde la autenticación y autorización hasta la persistencia de datos y la interacción con una base de datos.

## Arquitectura del Proyecto

La arquitectura de Garden API sigue los principios sólidos de diseño y buenas prácticas de desarrollo. Se estructura en capas para garantizar la modularidad, la flexibilidad y la facilidad de mantenimiento. Las principales capas son:

1. **Dominio (Domain):** Aquí se definen las entidades de dominio, que representan los conceptos clave del negocio, así como las interfaces y contratos necesarios para la implementación de la capa de infraestructura.

2. **Persistencia (Persistence):** Esta capa se encarga de la interacción con la base de datos. Aquí encontrarás el contexto de Entity Framework, las configuraciones de las entidades y las migraciones de base de datos.

3. **Aplicación (Application):** En esta capa se implementan las clases concretas de las interfaces definidas en el dominio. Incluye la implementación de repositorios, la unidad de trabajo y otros servicios de infraestructura.

4. **Presentación (Api):** Esta capa gestiona las solicitudes HTTP, maneja la autenticación y autorización mediante JWT, y orquesta las operaciones del dominio.

    - **DTOs (Data Transfer Objects):** Los DTOs facilitan la transferencia de datos entre capas y actúan como mensajes entre la capa de presentación y la capa de dominio.

    - **Controladores:** En esta subcapa se encuentran los controladores que manejan las solicitudes HTTP, interactúan con la capa de aplicación y devuelven las respuestas adecuadas.

## Contenido

1. [DbFirst y conexión con la base de datos](#dbfirst-y-conexión-con-la-base-de-datos)
   - [Conexión a la Base de Datos](#conexión-a-la-base-de-datos)
2. [Implementación de JWT en ASP.NET Core](#implementación-de-jwt-en-aspnet-core)
   - [Generación de Clave y Credenciales](#generación-de-clave-y-credenciales)
3. [Configuración del Entity Framework y el Contexto](#configuración-del-entity-framework-y-el-contexto)
   - [Clase GardenContext](#clase-GardenContext)
4. [Migraciones y Actualización de la Base de Datos](#migraciones-y-actualización-de-la-base-de-datos)
5. [Patrones de diseño en Garden API](#patrones-de-diseño-en-Garden-api)
   - [Unidad de Trabajo (UnitOfWork)](#unidad-de-trabajo-unitofwork)
   - [Repositorios](#repositorios)
   - [DTOs (Data Transfer Objects)](#dtos-data-transfer-objects)
6. [Controladores](#controladores)
7. [Servicios Adicionales](#servicios-adicionales)
   - [Serilog](#serilog)
8. [License](#license)

## DbFirst y conexión con la base de datos

### **DBFirst (Database First):**

Generas tus modelos y clases de entidades directamente desde la estructura de la base de datos existente.
Utilizas herramientas como Entity Framework CLI para generar automáticamente clases de entidades basadas en tablas de base de datos existentes.
Útil cuando ya tienes una base de datos establecida y deseas construir tu aplicación alrededor de ella.

### Conexión a la Base de Datos

Para conectarse a la base de datos de dbfirst utilizando Entity Framework Core y Pomelo.EntityFrameworkCore.MySql, sigue estos pasos:

1. **Instalación de Herramientas Necesarias:**
   Asegúrate de tener las herramientas necesarias instaladas. Si aún no lo has hecho, instala el Entity Framework Core y Pomelo.EntityFrameworkCore.MySql.

   ```bash
   dotnet tool install --global dotnet-ef --version 7.0.14
   ```

2. Generación del Contexto de la Base de Datos:

- Utiliza el siguiente comando para generar el contexto de la base de datos.

```bash
dotnet ef dbcontext scaffold "server=172.17.0.3;user=root;password=password;database=store" Pomelo.EntityFrameworkCore.MySql -s Api -p Persistence --context GardenContext --context-dir Data --output-dir Entities
```

Asegúrate de personalizar los parámetros de conexión según tu configuración.

3. Estructura de Directorios Generada:
Después de ejecutar el comando, la estructura de directorios se verá así:

```code
..
├── Persistence/
│   └── Data/
│       └── GardenContext.cs
└── Entities/
    ├── ModeloTabla1.cs
    ├── ModeloTabla2.cs
    └── ...
```

**GardenContext.cs**: Contiene la definición del contexto de la base de datos.

**Entities**: Directorio que contiene las clases de entidades generadas. Posteriormente exportaremos las entidades a la capa de Domain para continuar con el desarrollo de igual manera que con la aproximación Code-First.

## Implementación de JWT en ASP.NET Core

En este proyecto, se utiliza JSON Web Token (JWT) para autenticación y autorización en ASP.NET Core. A continuación, se presenta una descripción detallada del proceso.

### Generación de Clave y Credenciales

Para generar una clave secreta y credenciales para la firma JWT, se utiliza la clase `SymmetricSecurityKey` de ASP.NET Core. Esto asegura la integridad y seguridad del token.

```code
var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
var signingCredentials = new SigningCredentials(
    symmetricSecurityKey,
    SecurityAlgorithms.HmacSha256
);
```

**Configuración del appsettings.json para JWT:**

```json
{
  "JWT": {
    "Key": "njMCY^UbEskeAFL6eDzHuqY!s^x6Qrwe",
    "Issuer": "MyStoreApi",
    "Audience": "MyStoreApiUser",
    "DurationInMinutes": 1
  }
}
```

## Configuración del Entity Framework y el Contexto

A continuación, se presenta la configuración del Entity Framework y el contexto de la base de datos.

### Clase GardenContext

```csharp
using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data
{
    public class GardenContext : DbContext
    {
        public GardenContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Entity> Entities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
```

**Configuración del appsettings.json para Entity Framework:**

```json
{
  "ConnectionStrings": {
    "ConexMysql": "server=172.17.0.3;user=root;password=password;database=store"
  }
}
```

## Migraciones y Actualización de la Base de Datos

Para configurar tu base de datos, ejecuta los siguientes comandos en la terminal:

```bash
dotnet ef migrations add InitialCreate --project ./Persistence/ --startup-project ./Api/ --output-dir ./Data/Migrations

dotnet ef database update --project ./Persistence/ --startup-project ./Api/
```

Estos comandos generan una migración inicial y actualizan la base de datos según el contexto y las entidades definidas en tu proyecto.

## Patrones de diseño en Garden API

En este documento se describen los patrones de diseño utilizados en la Garden API. Estos patrones contribuyen a un diseño modular y mantenible al proporcionar una separación clara de responsabilidades, facilitar la implementación de operaciones de base de datos y optimizar la transferencia de datos entre las capas de la aplicación.

### Unidad de Trabajo (UnitOfWork)

La Unidad de Trabajo es un patrón de diseño que agrupa todas las operaciones de lectura y escritura en una única transacción. En el contexto de una base de datos, representa una sesión que mantiene el estado de las entidades y coordina su persistencia.

#### Función en Garden API:

En el proyecto, la Unidad de Trabajo, implementada a través de la interfaz IUnitOfWork, se encarga de gestionar las transacciones en la base de datos. Permite agrupar varias operaciones en una única transacción, lo que asegura la coherencia y la integridad de la base de datos.

```csharp
using Application.Repository;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly GardenContext _context;

        private IUser _users;

        public UnitOfWork(GardenContext context)
        {
            _context = context;
        }

        public IUser Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new UserRepository(_context);
                }
                return _users;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
```

## Repositorios

Los repositorios son patrones de diseño que encapsulan la lógica de acceso a los datos y proporcionan una interfaz para interactuar con la capa de persistencia. Cada entidad en la base de datos tiene su propio repositorio.

#### Función en Garden API:

Los repositorios en Garden API implementan la interfaz IRepository<T> definida en el dominio. Proporcionan métodos para realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) en la base de datos para entidades específicas, como Appointment, Breed, entre otras. Los repositorios abstraen la complejidad de las operaciones de base de datos y permiten interactuar con ellas de manera sencilla.

```csharp
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repository
{
    public class GenericRepository<T> : IGeneric<T>
        where T : class
    {
        private readonly GardenContext _context;

        public GenericRepository(GardenContext context)
        {
            _context = context;
        }

        public virtual void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetByNameAsync(string name)
        {
            return await _context.Set<T>().FindAsync(name);
        }

        public virtual void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public virtual void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
```

## DTOs (Data Transfer Objects)

Los DTOs son objetos utilizados para transferir datos entre capas de una aplicación. Se utilizan para encapsular la información y reducir la cantidad de datos transferidos entre la capa de presentación y la capa de dominio.

#### Función en Garden API:

En el proyecto, los DTOs, como EntityDTO, se emplean para estructurar y transferir datos entre la capa de presentación (controladores en la API) y la capa de dominio (entidades y servicios de aplicación). Facilitan la serialización de objetos complejos y permiten una comunicación eficiente entre las diferentes capas de la aplicación.

```csharp
namespace Api.Dtos
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
```

Con estos patrones y capas, Garden API en ASP.Net 7.0 está diseñado para brindar modularidad, mantenibilidad y escalabilidad. La arquitectura se organiza en capas bien definidas, cada una cumpliendo una función específica para garantizar un desarrollo eficiente y ordenado.

## Controladores

Controladores en ASP.NET Core
Los controladores en ASP.NET Core son clases que manejan las solicitudes HTTP y ejecutan la lógica de la aplicación correspondiente. Se utilizan para:

- **Gestión de Solicitudes HTTP:** Los controladores reciben solicitudes HTTP específicas (como GET, POST, PUT, DELETE) y ejecutan acciones correspondientes a esas solicitudes.

- **Interacción con Modelos y Vistas:** Los controladores trabajan con modelos y vistas para procesar y presentar datos. La lógica de la aplicación suele residir en los controladores, y estos interactúan con los modelos y las vistas para proporcionar una respuesta completa al cliente.

- **Enrutamiento:** ASP.NET Core utiliza enrutamiento para asociar solicitudes HTTP con controladores y acciones específicas. Los controladores juegan un papel importante en este proceso al definir acciones que responden a rutas específicas.

- **Inyección de Dependencias:** Los controladores admiten la inyección de dependencias, lo que facilita la prueba unitaria y la creación de aplicaciones más modulares y mantenibles.

- **Separación de Preocupaciones:** La arquitectura de ASP.NET Core fomenta la separación de preocupaciones. Los controladores se centran en la lógica de la aplicación, mientras que las vistas manejan la presentación y los modelos representan los datos.

- **Operaciones CRUD:** Los controladores son comúnmente utilizados para realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) en entidades de la aplicación.

## Servicios Adicionales

Además de las funcionalidades básicas proporcionadas por los controladores, Garden API en ASP.Net 7.0 incluye servicios adicionales para mejorar la seguridad y la eficiencia. Uno de estos servicios es:

### Serilog

Serilog es un servicio de registro que proporciona una forma flexible y estructurada de registrar información sobre el funcionamiento de la aplicación. Facilita el seguimiento y la solución de problemas al tiempo que ofrece opciones de salida personalizables.

#### Funcionalidades Principales:

- **Registros Estructurados:** Permite crear registros estructurados para facilitar la búsqueda y el análisis.

- **Múltiples Destinos:** Admite la configuración de múltiples destinos para los registros, como archivos, bases de datos o servicios externos.

- **Niveles de Severidad:** Clasifica los registros en diferentes niveles de severidad para una gestión eficiente de la información.

## License

Este proyecto está licenciado bajo la [ GNU General Public License v3.0](https://www.gnu.org/licenses/gpl-3.0.html) - consulta el archivo [LICENSE](LICENSE) para obtener más detalles.
