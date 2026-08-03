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

## Frontend

The frontend is built with TypeScript and communicates with the Minimal API using the Fetch API.

The frontend source code is located in:

```text
frontend/
```

### Requirements

* Node.js
* npm

Check that Node.js and npm are installed:

```bash
node --version
npm --version
```

### Install Dependencies

Open a terminal in the frontend directory:

```bash
cd frontend
```

Install the required npm packages:

```bash
npm install
```

This command reads `package.json` and installs the dependencies into the local `node_modules` directory.

### Start the Frontend

Start the Vite development server:

```bash
npm run dev
```

The terminal will display the local frontend address, usually:

```text
http://localhost:5173
```

Open that address in a web browser.

### API Connection

The frontend communicates with the API at:

```text
https://localhost:7065/api/courses
```

The API must be running before course data can be retrieved.

Start the API from the backend project directory:

```bash
dotnet run --launch-profile https
```

If HTTPS requests fail, make sure the ASP.NET Core development certificate is trusted:

```bash
dotnet dev-certs https --check --trust
```

### Frontend Development Commands

| Command           | Description                          |
| ----------------- | ------------------------------------ |
| `npm install`     | Install project dependencies         |
| `npm run dev`     | Start the development server         |
| `npm run build`   | Create a production build            |
| `npm run preview` | Preview the production build locally |

### Frontend Technologies

* TypeScript
* Vite
* HTML
* CSS


### Price Representation

The Price property was initially defined as decimal.

During development, this caused problems with SQLite when sorting courses by price. SQLite does not provide full native support for .NET decimal values, and decimal formatting can also cause confusion between:

* A period in JSON: 1025.50
* A comma in Swedish display formatting: 1025,50

JSON always requires a period as the decimal separator.

For this exercise, course prices are stored as whole Swedish kronor. The Price property was therefore changed from decimal to int.
