# Docker Deployment Guide

## Requisitos
- Docker Desktop instalado
- Docker Compose (incluido con Docker Desktop)

## Levantar toda la aplicación

### Opción 1: Levantar todo el stack (recomendado)
```bash
docker-compose up -d
```

### Opción 2: Compilar y levantar
```bash
docker-compose up -d --build
```

## Servicios disponibles

| Servicio | URL | Descripción |
|----------|-----|-------------|
| Frontend | http://localhost:3000 | Aplicación React |
| Backend API | http://localhost:5000 | API .NET |
| SQL Server | localhost:1433 | Base de datos |
| Kafka | localhost:9092 | Message broker |
| Elasticsearch | http://localhost:9200 | Motor de búsqueda |
| Zookeeper | localhost:2181 | Coordinador de Kafka |

## Comandos útiles

### Ver logs de todos los servicios
```bash
docker-compose logs -f
```

### Ver logs de un servicio específico
```bash
docker-compose logs -f backend
docker-compose logs -f frontend
```

### Detener todos los servicios
```bash
docker-compose down
```

### Detener y eliminar volúmenes (base de datos)
```bash
docker-compose down -v
```

### Reiniciar un servicio específico
```bash
docker-compose restart backend
```

### Ver el estado de los servicios
```bash
docker-compose ps
```

### Reconstruir un servicio específico
```bash
docker-compose up -d --build backend
```

## Inicialización de la base de datos

La base de datos se crea automáticamente en el primer arranque. Si necesitas ejecutar migraciones:

```bash
# Entrar al contenedor del backend
docker exec -it n5-permissions-backend bash

# Ejecutar migraciones (si están configuradas)
dotnet ef database update
```

## Configuración de producción

### Variables de entorno importantes

En `docker-compose.yml`, puedes modificar:

**SQL Server:**
- `SA_PASSWORD`: Contraseña del usuario sa (cambiar en producción)

**Backend:**
- `ConnectionStrings__DefaultConnection`: Cadena de conexión a SQL Server
- `Kafka__BootstrapServers`: Servidores de Kafka
- `Elasticsearch__Url`: URL de Elasticsearch

## Troubleshooting

### El backend no conecta a SQL Server
Espera unos segundos después de iniciar. SQL Server tarda en estar listo.

### Kafka no responde
Verifica que Zookeeper esté corriendo:
```bash
docker-compose logs zookeeper
```

### Frontend no carga
Verifica que el backend esté respondiendo:
```bash
curl http://localhost:5000/api/permissions
```

### Reiniciar desde cero
```bash
docker-compose down -v
docker-compose up -d --build
```

## Desarrollo local vs Docker

Para desarrollo local (sin Docker):
- Backend: `dotnet run` en `src/N5.Permissions.API`
- Frontend: `npm start` en `frontend/permissions-app`

El código en `Program.cs` detecta automáticamente el ambiente Development y mockea Kafka/Elasticsearch.

## Arquitectura de contenedores

```
┌──────────────┐
│   Frontend   │ :3000 (nginx)
└──────┬───────┘
       │
┌──────▼────────┐
│   Backend     │ :5000 (.NET)
└──┬──┬──┬──────┘
   │  │  │
   │  │  └──────► Elasticsearch :9200
   │  └─────────► Kafka :9092
   │                 └─► Zookeeper :2181
   └────────────► SQL Server :1433
```

## Producción

Para producción, considera:
1. Usar secretos en lugar de variables de entorno
2. Configurar SSL/TLS en el backend
3. Usar un reverse proxy (nginx/traefik) delante de todo
4. Configurar límites de recursos (CPU/memoria)
5. Implementar health checks
6. Configurar logging centralizado
7. Usar Docker Swarm o Kubernetes para orquestación
