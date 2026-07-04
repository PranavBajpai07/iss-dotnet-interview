# TODO API Refactoring Solution

**Candidate Name:** Pranav Bajpai  
**Completion Date:** 04/07/2026  

---

## Problems Identified

After reviewing the existing implementation, I identified several architectural, maintainability, and testing concerns.

### Architecture & Design

- Controllers were directly instantiating service classes using `new TodoService()` instead of leveraging Dependency Injection.
- Business logic, data access, and API concerns were tightly coupled.
- The API followed an action-based approach (`/createTodo`, `/updateTodo`, etc.) rather than a resource-based REST design.
- All operations were implemented using `POST`, including retrieval and deletion, which did not align with HTTP standards.

### Code Quality & Maintainability

- Lack of interface abstractions made the application difficult to test and extend.
- Request models and persistence models were not clearly separated.
- Database initialization logic was embedded directly in `Program.cs`.
- Repeated code patterns existed within controller actions.
- Limited validation increased the risk of bad data entering the system.

### Security & Reliability

- Raw exception messages could potentially be exposed to API consumers.
- Input validation was minimal.
- No centralized error-handling mechanism existed.

### Performance Concerns

- Synchronous-style patterns could become problematic as the application scales.
- Database access was tightly coupled to controllers, making optimization difficult.
- No separation between read and write concerns.

### Testing Gaps

- Existing tests focused primarily on implementation details rather than actual application behavior.
- Negative scenarios such as invalid IDs, invalid payloads, and delete failures were not fully covered.
- No integration-style testing existed to validate real API behavior.

---

## Architectural Decisions

To improve maintainability, testability, and scalability, I refactored the application into a layered architecture while keeping the solution lightweight and appropriate for the scope of the assignment.

### Layered Architecture

```text
Controller
    ↓
Service Interface
    ↓
Service Implementation
    ↓
EF Core DbContext
    ↓
SQLite Database
```

### Design Patterns Applied

#### Dependency Injection

Introduced ASP.NET Core's built-in Dependency Injection container and registered services through `Program.cs`.

Benefits:
- Reduces coupling
- Improves testability
- Supports extensibility

#### Service Layer Pattern

Business logic was moved from controllers into a dedicated service layer.

Benefits:
- Thin controllers
- Better separation of concerns
- Easier maintenance

#### DTO Pattern

Added dedicated DTOs:

- CreateTodoRequest
- UpdateTodoRequest
- TodoResponse

Benefits:
- Prevents exposing database entities directly
- Supports validation
- Enables future API versioning

### Technology Choices

#### Entity Framework Core

Replaced manual database handling with EF Core.

Benefits:
- Cleaner data access
- Built-in change tracking
- Simplified CRUD operations
- Better testing support

#### SQLite

Retained SQLite for simplicity and ease of local execution.

Benefits:
- No external database dependency
- Lightweight
- Easy setup

#### Integration Testing

Added API-level tests using:

- xUnit
- WebApplicationFactory
- SQLite In-Memory database

Benefits:
- Tests real behavior
- Covers routing, validation, persistence and responses
- More confidence than unit tests alone

### Project Structure

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

## Trade-offs

During the refactoring process, I made deliberate trade-offs to balance simplicity and maintainability.

### What I Prioritized

- RESTful API design
- Dependency Injection
- Separation of concerns
- Meaningful test coverage
- Readability and maintainability
- Production-oriented coding practices

### What I Deferred

#### Clean Architecture

I considered implementing:

- Domain Layer
- Application Layer
- Infrastructure Layer

However, given the small scope of the application, this would have added unnecessary complexity.

#### CQRS

These patterns are valuable for larger systems but would increase complexity without providing significant value for a CRUD-based TODO API.

#### Repository Pattern

Entity Framework Core already provides Repository and Unit of Work behavior through:

- DbContext
- DbSet

Adding another repository layer would have introduced additional abstraction without clear benefits.

### Alternatives Considered

| Option | Decision |
|----------|----------|
| Repository Pattern | Not implemented |
| CQRS | Not implemented |
| AutoMapper | Not implemented |
| FluentValidation | Deferred |
| Clean Architecture | Deferred |

The final solution favors simplicity while still demonstrating production-ready design principles.

---

## How to Run

### Prerequisites

- .NET 8 SDK
- Git
- Visual Studio 2022 / Rider / VS Code

### Build

```bash
dotnet restore
dotnet build
```

### Run

```bash
dotnet run --project TodoApi
```

Swagger will be available at:

```text
https://localhost:<port>/swagger
```

### Test

```bash
dotnet test
```

---

## API Documentation

### Create TODO

**Method**

```http
POST /api/todos
```

**Request Body**

```json
{
  "title": "Prepare Interview Solution",
  "description": "Refactor TODO API"
}
```

**Response**

```json
{
  "id": 1,
  "title": "Prepare Interview Solution",
  "description": "Refactor TODO API",
  "isCompleted": false,
  "createdAt": "2026-07-04T12:00:00Z"
}
```

**Status Code**

```http
201 Created
```

---

### Get TODO(s)

#### Get All

**Method**

```http
GET /api/todos
```

**Response**

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

#### Get By Id

**Method**

```http
GET /api/todos/1
```

**Response**

```json
{
  "id": 1,
  "title": "Prepare Interview Solution",
  "description": "Refactor TODO API",
  "isCompleted": false
}
```

**Status Codes**

```http
200 OK
404 Not Found
```

---

### Update TODO

**Method**

```http
PUT /api/todos/1
```

**Request Body**

```json
{
  "title": "Prepare Final Submission",
  "description": "Include documentation",
  "isCompleted": true
}
```

**Response**

```json
{
  "id": 1,
  "title": "Prepare Final Submission",
  "description": "Include documentation",
  "isCompleted": true
}
```

**Status Codes**

```http
200 OK
404 Not Found
```

---

### Delete TODO

**Method**

```http
DELETE /api/todos/1
```

**Response**

```http
204 No Content
```

**Status Codes**

```http
204 No Content
404 Not Found
```

---

## Future Improvements

If additional time were available, I would enhance the application in the following areas:

### Architecture

- Introduce Clean Architecture structure
- Consider CQRS for command/query separation
- Add domain-level abstractions

### API Improvements

- Pagination support
- Filtering and searching
- Sorting capabilities
- Partial updates using PATCH

### Error Handling

- Global exception handling middleware
- ProblemDetails responses
- Consistent error contracts

### Validation

- Implement FluentValidation
- Custom validation rules
- Business rule validation layer

### Security

- JWT Authentication
- Role-based Authorization
- API versioning
- Rate limiting

### Observability

- Structured logging using Serilog
- Correlation IDs
- Distributed tracing
- Health checks

### Testing

- Additional service-level unit tests
- Load testing
- Contract testing
- Code coverage reporting

### DevOps & Deployment

- Docker support
- CI/CD pipeline
- GitHub Actions / Azure DevOps
- Automated quality gates

### Database

- Replace `EnsureCreated()` with EF Core Migrations
- Migration version tracking
- Seed data strategy

---

## Conclusion

The primary focus of this refactoring exercise was to transform a functional but tightly coupled implementation into a maintainable, testable, and production-oriented ASP.NET Core application.

The final solution introduces:

- RESTful endpoint design
- Dependency Injection
- Service abstraction
- DTO-based contracts
- EF Core integration
- Validation
- Integration testing
- Improved maintainability

While intentionally avoiding over-engineering, the application now follows modern ASP.NET Core best practices and provides a solid foundation for future enhancements.