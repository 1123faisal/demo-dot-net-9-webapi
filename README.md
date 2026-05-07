# MyFirstWebApi

A RESTful Web API built with **ASP.NET Core (.NET 10)** demonstrating basic CRUD operations on a `Products` resource. It uses in-memory data storage and exposes interactive API documentation via **Scalar**.

---

## Features

- Full CRUD operations on products (`GET`, `POST`, `PUT`, `DELETE`)
- Product search with optional filters (`category`, `maxPrice`)
- OpenAPI specification via `Microsoft.AspNetCore.OpenApi`
- Interactive API explorer via **Scalar** (available in Development mode)
- HTTPS redirection and authorization middleware

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 |
| Language | C# (.NET 10) |
| API Docs | Microsoft.AspNetCore.OpenApi `10.0.3` |
| API Explorer | Scalar.AspNetCore `2.14.11` |
| Data Store | In-memory (no database) |

---

## Project Structure

```
dot-net-demos.sln
MyFirstWebApi/
├── Controllers/
│   └── ProductsController.cs   # Products API controller + Product model
├── Properties/
│   └── launchSettings.json     # Launch profiles (http / https)
├── appsettings.json             # App configuration
├── appsettings.Development.json # Development-specific configuration
├── Program.cs                   # Application entry point & middleware pipeline
└── MyFirstWebApi.csproj         # Project file & NuGet dependencies
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Run the API

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

While running in **Development** mode, open:

```
http://localhost:5056/scalar/v1
```

---

## API Endpoints

Base URL: `/api/products`

### Get All Products

```http
GET /api/products
```

Returns a list of all products.

---

### Get Product by ID

```http
GET /api/products/{id}
```

| Parameter | Type | Description |
|---|---|---|
| `id` | `int` | Product ID |

Returns the product, or `404 Not Found` if it does not exist.

---

### Search Products

```http
GET /api/products/search?category={category}&maxPrice={maxPrice}
```

| Query Param | Type | Description |
|---|---|---|
| `category` | `string` | Filter by category (optional) |
| `maxPrice` | `double` | Maximum price filter (optional) |

---

### Create Product

```http
POST /api/products
Content-Type: application/json
```

**Request Body:**

```json
{
  "name": "Chair",
  "price": 4500,
  "category": "Furniture"
}
```

Returns `201 Created` with the created product.

---

### Update Product

```http
PUT /api/products/{id}
Content-Type: application/json
```

**Request Body:**

```json
{
  "name": "Gaming Chair",
  "price": 8500,
  "category": "Furniture"
}
```

Returns the updated product, or `404 Not Found`.

---

### Delete Product

```http
DELETE /api/products/{id}
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
| `id` | `int` | Unique identifier (auto-assigned on create) |
| `name` | `string` | Product name |
| `price` | `double` | Product price |
| `category` | `string` | Product category |

---

## Seed Data

The API ships with the following in-memory seed data:

| ID | Name | Price | Category |
|---|---|---|---|
| 1 | Laptop | 75000 | Electronics |
| 2 | Phone | 25000 | Electronics |
| 3 | Desk | 12000 | Furniture |
| 4 | Notebook | 200 | Stationery |

> **Note:** Data is reset every time the application restarts (no persistent storage).

---

## License

This project is intended for demo and learning purposes.
