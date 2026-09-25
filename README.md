# N5 Permissions - Sistema de Gestión de Permisos

Sistema de gestión de permisos de empleados construido con .NET 10 (Backend) y React (Frontend), utilizando arquitectura limpia, CQRS, Kafka y Elasticsearch.

## 🚀 Características

- **Backend**: .NET 10 con Clean Architecture (Domain, Application, Infrastructure, API)
- **Frontend**: React 18 con Material-UI
- **Base de Datos**: SQL Server 2022
- **Message Broker**: Apache Kafka
- **Motor de Búsqueda**: Elasticsearch
- **Patrones**: CQRS con MediatR, Repository Pattern, Unit of Work
- **Orquestación**: Docker Compose

## 📋 Requisitos Previos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado y corriendo
- Git instalado (opcional, para clonar el repositorio)

## 🏗️ Arquitectura

```
┌──────────────┐
│   Frontend   │ React + Material-UI (Puerto 3000)
│   (nginx)    │
└──────┬───────┘
       │
┌──────▼────────┐
│   Backend     │ .NET 10 Web API (Puerto 5000)
│   API Layer   │
└──┬──┬──┬──────┘
   │  │  │
   │  │  └─────────► Elasticsearch (Puerto 9200)
   │  │                - Indexación de permisos
   │  │
   │  └────────────► Kafka + Zookeeper (Puerto 9092)
   │                  - Publicación de eventos
   │
   └───────────────► SQL Server 2022 (Puerto 1433)
                      - Almacenamiento principal
```

## 🚀 Instalación y Uso

### Opción 1: Usando Docker Compose (Recomendado)

1. **Clonar el repositorio**
```bash
git clone https://github.com/APITHEBEST02/N5-Permissions.git
cd N5-Permissions
```

2. **Levantar todos los servicios**
```bash
docker compose up -d
```

Este comando descargará todas las imágenes necesarias y levantará 6 contenedores:
- Frontend (React)
- Backend (.NET)
- SQL Server
- Kafka
- Zookeeper
- Elasticsearch

3. **Inicializar la base de datos** (solo la primera vez)

Los ejemplos usan `<CONTRASENA_LOCAL>` como marcador: reemplázalo únicamente en tu entorno local con el valor configurado para SQL Server. No copies credenciales reales al repositorio.
```bash
# Windows PowerShell
Get-Content init-db.sql | docker exec -i n5-permissions-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "<CONTRASENA_LOCAL>" -C

# Linux/Mac
cat init-db.sql | docker exec -i n5-permissions-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "<CONTRASENA_LOCAL>" -C
```

4. **Acceder a la aplicación**
- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000
- **Swagger**: http://localhost:5000/swagger

### Opción 2: Desarrollo Local (sin Docker)

#### Backend
```bash
cd src/N5.Permissions.API
dotnet restore
dotnet run
```

#### Frontend
```bash
cd frontend/permissions-app
npm install
npm start
```

> **Nota**: En modo Development, Kafka y Elasticsearch están mockeados automáticamente.

## 📦 Servicios y Puertos

| Servicio | Puerto | Descripción |
|----------|--------|-------------|
| Frontend | 3000 | Aplicación React |
| Backend API | 5000 | API .NET |
| SQL Server | 1433 | Base de datos |
| Kafka | 9092 | Message broker |
| Elasticsearch | 9200, 9300 | Motor de búsqueda |
| Zookeeper | 2181 | Coordinador Kafka |

## 🛠️ Comandos Útiles de Docker

```bash
# Ver estado de los contenedores
docker compose ps

# Ver logs de todos los servicios
docker compose logs -f

# Ver logs de un servicio específico
docker compose logs -f backend
docker compose logs -f frontend

# Detener todos los servicios
docker compose down

# Detener y eliminar volúmenes (reinicio completo)
docker compose down -v

# Reiniciar un servicio específico
docker compose restart backend

# Reconstruir imágenes después de cambios en el código
docker compose up -d --build
```

## 🗃️ Base de Datos

### Configuración local (Docker)
- **Server**: localhost,1433
- **Usuario**: sa
- **Password**: utiliza una contraseña local; no publiques valores reales.
- **Base de datos**: PermissionsDB

### Tablas
- **PermissionTypes**: Tipos de permisos (Vacaciones, Enfermedad, etc.)
- **Permissions**: Permisos solicitados por empleados

## 📱 Uso de la Aplicación

1. **Ver Permisos**: Lista todos los permisos solicitados
2. **Solicitar Permiso**: Crear un nuevo permiso
   - Nombre del empleado
   - Apellido del empleado
   - Tipo de permiso
   - Fecha del permiso
3. **Modificar Permiso**: Editar un permiso existente
4. **Gestionar Tipos**: Crear y listar tipos de permisos

## 🏛️ Estructura del Proyecto

```
├── src/
│   ├── N5.Permissions.API/          # Capa de presentación (Controllers)
│   ├── N5.Permissions.Application/  # Lógica de aplicación (CQRS, Handlers)
│   ├── N5.Permissions.Domain/       # Entidades de dominio
│   ├── N5.Permissions.Infrastructure/ # Repositorios, DbContext
│   └── N5.Permissions.Shared/       # DTOs, Utilidades
├── frontend/
│   └── permissions-app/             # Aplicación React
├── tests/
│   ├── N5.Permissions.UnitTests/    # Unit Tests
│   └── N5.Permissions.IntegrationTests/ # Integration Tests
├── docker-compose.yml               # Orquestación de contenedores
├── init-db.sql                      # Script de inicialización DB
└── README.md
```

## 🧪 Pruebas

### Unit Tests
```bash
cd tests/N5.Permissions.UnitTests
dotnet test
```

### Integration Tests
```bash
cd tests/N5.Permissions.IntegrationTests
dotnet test
```

## 🔧 Variables de Entorno

### Backend (Production)
```env
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Server=sqlserver;Database=PermissionsDB;User Id=sa;Password=<CONTRASENA_LOCAL>;TrustServerCertificate=True;
Kafka__BootstrapServers=kafka:29092
Kafka__Topic=permissions
Elasticsearch__Url=http://elasticsearch:9200
Elasticsearch__DefaultIndex=permissions
```

### Backend (Development)
```env
ASPNETCORE_ENVIRONMENT=Development
# En Development, Kafka y Elasticsearch están mockeados automáticamente
```

## 🐛 Troubleshooting

### El backend no responde
```bash
# Ver logs del backend
docker compose logs backend

# Reiniciar el backend
docker compose restart backend
```

### La base de datos no existe
```bash
# Ejecutar el script de inicialización
Get-Content init-db.sql | docker exec -i n5-permissions-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "<CONTRASENA_LOCAL>" -C
```

### Kafka no responde
```bash
# Verificar que Zookeeper está corriendo
docker compose ps zookeeper

# Reiniciar Kafka
docker compose restart kafka
```

### Error "puerto ya en uso"
```bash
# Detener todos los servicios
docker compose down

# Verificar que no hay procesos usando los puertos
netstat -ano | findstr :5000
netstat -ano | findstr :3000

# Levantar de nuevo
docker compose up -d
```

### Reinicio completo del sistema
```bash
# Detener todo y eliminar volúmenes
docker compose down -v

# Reconstruir y levantar
docker compose up -d --build

# Reinicializar la base de datos
Get-Content init-db.sql | docker exec -i n5-permissions-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "<CONTRASENA_LOCAL>" -C
```

## 📚 Tecnologías

### Backend
- .NET 10.0
- Entity Framework Core 10.0.2
- MediatR 14.0.0 (CQRS)
- FluentValidation 12.1.1
- Confluent.Kafka 2.13.0
- NEST 7.17.5 (Elasticsearch)

### Frontend
- React 18
- Material-UI (@mui/material)
- Axios
- React Hooks

### Infraestructura
- Docker & Docker Compose
- SQL Server 2022
- Apache Kafka 7.5.0
- Elasticsearch 7.17.15
- Nginx (servidor web del frontend)

## 👥 Contribuir

1. Fork el proyecto
2. Crea tu rama de feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📝 Licencia

Actualmente no hay un archivo LICENSE en este repositorio. Las condiciones de uso y redistribución están pendientes de definición por el autor.

## 📞 Contacto

Para preguntas o sugerencias, por favor abre un issue en el repositorio.

---

⭐ Si este proyecto te fue útil, considera darle una estrella en GitHub!
