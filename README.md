# Course Portal Mini API

> **Educational project:** This API was created as a small group exercise to practise ASP.NET Core Minimal API, Entity Framework Core, SQLite, DTOs, validation, and HTTP requests from a TypeScript frontend.

## Overview

Course Portal Mini API provides basic CRUD operations for courses:

* Retrieve all courses
* Retrieve a course by ID
* Create a course
* Update a course
* Delete a course

## Technologies

* .NET 10
* ASP.NET Core Minimal API
* Entity Framework Core
* SQLite
* OpenAPI
* Swagger UI

## Run the API

Restore and build the project:

```bash
dotnet restore
dotnet build
```

Start the API using the HTTPS profile:

```bash
dotnet run --launch-profile https
```

API address:

```text
https://localhost:7065
```

Swagger UI:

```text
https://localhost:7065/swagger
```

## Database

The project uses a local SQLite database:

```text
courseportal.db
```

Pending migrations are applied when the application starts. The database is seeded with example courses when the `Courses` table is empty.

Create a new migration after changing the database model:

```bash
dotnet ef migrations add MigrationName
```

Apply migrations manually:

```bash
dotnet ef database update
```

## API Endpoints

| Method   | Endpoint            | Description             |
| -------- | ------------------- | ----------------------- |
| `GET`    | `/api/courses`      | Retrieve all courses    |
| `GET`    | `/api/courses/{id}` | Retrieve a course by ID |
| `POST`   | `/api/courses`      | Create a course         |
| `PUT`    | `/api/courses/{id}` | Update a course         |
| `DELETE` | `/api/courses/{id}` | Delete a course         |

Example request body for `POST` and `PUT`:

```json
{
  "name": "TypeScript and API Integration",
  "description": "Learn how to communicate with a REST API using TypeScript and fetch.",
  "startDate": "2026-09-14",
  "durationInWeeks": 8,
  "price": 4995,
  "level": 2
}
```

Course levels:

```text
1 = Beginner
2 = Intermediate
3 = Advanced
```

## Validation

The API validates course names, descriptions, duration, price, and course level.

The combination of `Name` and `StartDate` must be unique.

## CORS

The current CORS configuration is intended for local development.

Before production deployment, replace unrestricted origins, methods, and headers with explicitly approved frontend addresses and required HTTP methods.

## Project Structure

```text
CoursePortalMiniApi/
├── Constants/
├── Data/
├── DTOs/
├── Enums/
├── Migrations/
├── Models/
├── Properties/
├── Program.cs
├── appsettings.json
└── CoursePortalMiniApi.csproj
```

## Formatting

Check formatting without modifying files:

```bash
dotnet format CoursePortalMiniApi.csproj \
  --verify-no-changes \
  --verbosity minimal
```

