# MyFirstWebApi

A RESTful Web API built with **ASP.NET Core (.NET 10)** demonstrating CRUD operations on a `Products` resource with JWT-based authentication. Uses **SQLite** for persistent storage and exposes interactive API documentation via **Scalar**.

---

## Features

- Full CRUD operations on products (`GET`, `POST`, `PUT`, `DELETE`)
- Product search with optional filters (`category`, `maxPrice`)
- JWT authentication with register/login endpoints
- Role-based authorization on protected endpoints
- OpenAPI specification with Bearer token security scheme
- Interactive API explorer via **Scalar** with lock icon on protected routes
- SQLite persistent storage with Entity Framework Core migrations
- Docker support with data volume mounting

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 |
| Language | C# (.NET 10) |
| Database | SQLite via Entity Framework Core 10 |
| Authentication | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| API Docs | `Microsoft.AspNetCore.OpenApi` 10.0.3 |
| API Explorer | `Scalar.AspNetCore` 2.14.11 |

---

## Project Structure

```
dot-net-demos.sln
MyFirstWebApi/
├── Controllers/
│   ├── AuthController.cs        # Register & login endpoints
│   └── ProductsController.cs   # Products CRUD controller
├── Data/
│   └── AppDbContext.cs          # EF Core DbContext
├── Migrations/                  # EF Core migration files
├── Models/
│   └── User.cs                  # User entity model
├── Services/
│   ├── IAuthService.cs          # Auth service interface
│   └── AuthService.cs           # JWT auth implementation
├── Properties/
│   └── launchSettings.json      # Launch profiles
├── Dockerfile                   # Docker image definition
├── appsettings.json             # App configuration (DB, JWT)
├── appsettings.Development.json # Development overrides
├── Program.cs                   # Entry point & middleware pipeline
└── MyFirstWebApi.csproj         # Project file & NuGet dependencies
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/) (optional)

### Run Locally

```bash
cd MyFirstWebApi
dotnet run
```

The API will be available at:

| Profile | URL |
|---|---|
| HTTP  | `http://localhost:5056` |
| HTTPS | `https://localhost:7136` |

### Interactive API Docs (Scalar)

Open in your browser while running in Development mode:

```
http://localhost:5056/scalar/v1
```

Endpoints marked with a **lock icon** require a Bearer token. Use the lock button to enter your JWT before making requests.

---

## Running with Docker

### Build the image

```bash
docker build -t myfirstapi .
```

### Run the container

```powershell
docker run -p 8080:8080 -v ${PWD}/data:/app/data myfirstapi
```

The API will be available at `http://localhost:8080`.  
Scalar UI: `http://localhost:8080/scalar/v1`

The SQLite database is persisted to the `./data` folder on the host via the volume mount.

---

## Configuration

### appsettings.json

| Key | Description |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQLite database path (default: `/app/data/app.db`) |
| `Jwt:Key` | Secret key for signing JWT tokens |
| `Jwt:Issuer` | JWT issuer |
| `Jwt:Audience` | JWT audience |
| `Jwt:ExpiryHours` | Token expiry in hours (default: `24`) |

---

## API Endpoints

### Authentication

#### Register

```http
POST /api/auth/register
Content-Type: application/json
```

```json
{
  "username": "alice",
  "password": "secret123",
  "role": "Admin"
}
```

Returns a JWT token on success.

---

#### Login

```http
POST /api/auth/login
Content-Type: application/json
```

```json
{
  "username": "alice",
  "password": "secret123"
}
```

Returns a JWT token on success.

---

### Products

Base URL: `/api/products`

> Protected endpoints require `Authorization: Bearer <token>` header.

#### Get All Products

```http
GET /api/products
```

#### Get Product by ID

```http
GET /api/products/{id}
```

#### Search Products

```http
GET /api/products/search?category={category}&maxPrice={maxPrice}
```

| Query Param | Type | Description |
|---|---|---|
| `category` | `string` | Filter by category (optional) |
| `maxPrice` | `double` | Maximum price filter (optional) |

#### Create Product 🔒

```http
POST /api/products
Authorization: Bearer <token>
Content-Type: application/json
```

```json
{
  "name": "Chair",
  "price": 4500,
  "category": "Furniture"
}
```

Returns `201 Created` with the created product.

#### Update Product 🔒

```http
PUT /api/products/{id}
Authorization: Bearer <token>
Content-Type: application/json
```

```json
{
  "name": "Gaming Chair",
  "price": 8500,
  "category": "Furniture"
}
```

Returns the updated product, or `404 Not Found`.

#### Delete Product 🔒

```http
DELETE /api/products/{id}
Authorization: Bearer <token>
```

Returns `204 No Content` on success, or `404 Not Found`.

---

## Product Model

```json
{
  "id": 1,
  "name": "Laptop",
  "price": 75000,
  "category": "Electronics"
}
```

| Field | Type | Description |
|---|---|---|
| `id` | `int` | Unique identifier (auto-assigned) |
| `name` | `string` | Product name |
| `price` | `double` | Product price |
| `category` | `string` | Product category |

---

## Seed Data

On startup, the database is seeded with the following products if empty:

| Name | Price | Category |
|---|---|---|
| Laptop | 75,000 | Electronics |
| Phone | 25,000 | Electronics |
| Headphones | 3,000 | Electronics |
| Desk | 12,000 | Furniture |
| Notebook | 200 | Stationery |

