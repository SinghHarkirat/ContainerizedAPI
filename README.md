# ContainerizedAPI

ContainerizedAPI is a .NET 8 Web API project designed for modern, scalable, and maintainable application development. It leverages Entity Framework Core for data access and is fully containerized using Docker, making it easy to deploy and run in any environment.

## Features
- ASP.NET Core Web API built on .NET 8
- Entity Framework Core with SQL Server support
- Docker and Docker Compose support for containerized development and deployment
- Organized project structure with separation of concerns (DTOs, Entities, Migrations)
- Ready for cloud or on-premises deployment

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)

### Running the API Locally
1. Clone the repository.
2. Navigate to the project directory.
3. Build and run the API using Docker Compose:
   ```powershell
   docker-compose up --build
   ```
4. The API will be available at `http://localhost:5000` (or as configured).

### Project Structure
- `src/` - Source code for the API
  - `Entities/` - Entity models
  - `DTOs/` - Data Transfer Objects
  - `Migrations/` - Entity Framework Core migrations
  - `Program.cs` - Application entry point
  - `MyDbContext.cs` - Entity Framework Core DbContext
- `docker-compose.yml` - Docker Compose configuration
- `Dockerfile` - Docker build instructions

## Database Migrations
To add a new migration:
```powershell
dotnet ef migrations add <MigrationName> --project src
```
To update the database:
```powershell
dotnet ef database update --project src
```

## License
This project is licensed under the MIT License.
