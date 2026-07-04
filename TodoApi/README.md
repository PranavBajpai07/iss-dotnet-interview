# TODO API

A refactored TODO API built with ASP.NET Core 8.0, Entity Framework Core, SQLite, and RESTful API principles.

---

# Overview

This project was refactored to improve:

- Clean architecture and separation of concerns
- Dependency Injection
- RESTful API design
- Code maintainability
- Testability
- Input validation
- Integration test coverage

The application provides CRUD operations for TODO items and uses SQLite as the persistence layer.

---

# Architecture

The application follows a layered architecture:

```text
Controller
    ↓
Service Interface
    ↓
Service Implementation
    ↓
Entity Framework Core DbContext
    ↓
SQLite Database
```

Project Structure:

```text
TodoApi/
├── Controllers/
│   └── TodosController.cs
├── Data/
│   └── TodoDbContext.cs
├── Dtos/
│   ├── CreateTodoRequest.cs
│   ├── UpdateTodoRequest.cs
│   └── TodoResponse.cs
├── Models/
│   └── Todo.cs
├── Services/
│   ├── ITodoService.cs
│   └── TodoService.cs
├── Program.cs
└── appsettings.json

TodoApi.Tests/
├── Api/
│   └── TodosApiTests.cs
└── TestHelpers/
    └── CustomWebApplicationFactory.cs
```

---

# Technologies Used

- ASP.NET Core 8.0
- Entity Framework Core 8
- SQLite
- Swagger / OpenAPI
- xUnit
- Microsoft.AspNetCore.Mvc.Testing

---

# Getting Started

## Prerequisites

- .NET 8 SDK

Verify installation:

```bash
dotnet --version
```

---

## Restore Packages

```bash
dotnet restore
```

---

## Build the Solution

```bash
dotnet build
```

---

## Run the Application

From the repository root:

```bash
dotnet run --project TodoApi
```

The API will start on:

```text
https://localhost:<port>   (or check the console output for the exact URL)
```

or

```text
http://localhost:<port>    (or check the console output for the exact URL)
```

depending on your local environment.

---

## Swagger

Swagger UI is enabled in Development mode.

Open:

```text
https://localhost:<port>/swagger    (or check the console output for the exact URL)
```

---

# API Endpoints

Base Route:

```text
/api/todos
```

---

## Create TODO

### Request

```http
POST /api/todos
```

### Body

```json
{
  "title": "Prepare Interview Solution",
  "description": "Refactor TODO API"
}
```

### Response

```json
{
  "id": 1,
  "title": "Prepare Interview Solution",
  "description": "Refactor TODO API",
  "isCompleted": false,
  "createdAt": "2026-07-04T12:00:00Z"
}
```

### Status Codes

```http
201 Created
400 Bad Request
```

---

## Get All TODOs

### Request

```http
GET /api/todos
```

### Response

```json
[
  {
    "id": 1,
    "title": "Prepare Interview Solution",
    "description": "Refactor TODO API",
    "isCompleted": false
  }
]
```

### Status Code

```http
200 OK
```

---

## Get TODO By Id

### Request

```http
GET /api/todos/1
```

### Response

```json
{
  "id": 1,
  "title": "Prepare Interview Solution",
  "description": "Refactor TODO API",
  "isCompleted": false
}
```

### Status Codes

```http
200 OK
404 Not Found
```

---

## Update TODO

### Request

```http
PUT /api/todos/1
```

### Body

```json
{
  "title": "Prepare Final Submission",
  "description": "Final review completed",
  "isCompleted": true
}
```

### Response

```json
{
  "id": 1,
  "title": "Prepare Final Submission",
  "description": "Final review completed",
  "isCompleted": true
}
```

### Status Codes

```http
200 OK
404 Not Found
400 Bad Request
```

---

## Delete TODO

### Request

```http
DELETE /api/todos/1
```

### Response

```http
204 No Content
```

### Status Codes

```http
204 No Content
404 Not Found
```

---

# Validation Rules

### CreateTodoRequest

| Field | Validation |
|---------|------------|
| Title | Required |
| Title | Maximum 200 characters |
| Description | Maximum 1000 characters |

### UpdateTodoRequest

| Field | Validation |
|---------|------------|
| Title | Required |
| Title | Maximum 200 characters |
| Description | Maximum 1000 characters |

---

# Running Tests

Execute all tests:

```bash
dotnet test
```

The test suite includes:

- Create TODO (success)
- Create TODO (validation failure)
- Get all TODOs
- Get TODO by ID
- Get TODO by invalid ID
- Update TODO
- Update invalid TODO
- Delete TODO
- Delete invalid TODO

---

# Database

The application uses:

```text
SQLite
```

Database file:

```text
todos.db
```

The database is automatically created during application startup if it does not exist.

Connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=todos.db"
  }
}
```

---

# Improvements Introduced During Refactoring

### API Improvements

- Replaced action-based endpoints with RESTful endpoints
- Proper HTTP verbs used (GET, POST, PUT, DELETE)
- Consistent status code usage

### Architecture Improvements

- Introduced Dependency Injection
- Added Service Layer abstraction
- Added DTO layer
- Added EF Core DbContext

### Quality Improvements

- Input validation
- Async database operations
- Improved maintainability
- Better separation of concerns

### Testing Improvements

- Integration-style API testing
- Positive test scenarios
- Negative test scenarios
- In-memory SQLite testing

---

# Future Enhancements

Potential improvements if the system evolves further:

- Global Exception Handling Middleware
- Structured Logging (Serilog)
- API Versioning
- Authentication & Authorization
- Pagination & Filtering
- Docker Support
- CI/CD Pipeline
- EF Core Migrations
- Health Checks
- Rate Limiting

---

# Author

Pranav Bajpai

Refactored as part of the TODO API Technical Assessment.