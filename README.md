A .NET 9 Web API for managing CRM contacts, built with a clean layered architecture (AppCore / Infrastructure / WebApi).

## Features

- Contact management — create, read, update, and delete persons and organizations, with support for tags and notes
- JWT authentication — login, registration, token refresh, and revocation using ASP.NET Core Identity
- Role-based authorization — five built-in roles: Administrator, SalesManager, Salesperson, SupportAgent, ReadOnly
- KRS validation — optional validation of Polish KRS numbers against the public government API when creating organizations
- Dual storage backends — Entity Framework Core (SQLite) for production, in-memory repositories for development/testing
- FluentValidation — request validation for registration and person creation (including Polish phone numbers, age checks, and password strength)
- Unit tests — xUnit tests covering the generic repository (add, update, delete, paging, error cases)

## Tech Stack

| Layer | Technologies |
|---|---|
| Runtime | .NET 9 |
| Web framework | ASP.NET Core |
| ORM | Entity Framework Core 9 + SQLite |
| Identity | ASP.NET Core Identity |
| Auth | JWT Bearer tokens + Refresh tokens |
| Validation | FluentValidation |
| Mapping | AutoMapper |
| Testing | xUnit |

## Project Structure
CoreApp.sln
├── AppCore/          # Domain entities, DTOs, interfaces, validators, service contracts
├── Infrastructure/   # EF Core repositories, Identity, JWT auth service, in-memory repos
├── WebApi/           # ASP.NET Core controllers, middleware, startup
└── UnitTests/        # xUnit tests
## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Run the API
cd WebApi
dotnet run
The API starts at http://localhost:5059. On first run it creates the SQLite database (`crm.db`) and seeds an admin user.

Default admin credentials:

Ema
il:    admin@crm.pl
Password: Admin@123!

## API Reference

### Authentication

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| POST | /api/auth/register | Public | Register a new user |
| POST | /api/auth/login | Public | Login and receive tokens |
| POST | /api/auth/refresh | Public | Refresh an access token |
| POST | /api/auth/revoke | Bearer | Revoke a refresh token |
| GET | /api/auth/me | Bearer | Get current user info |

### Contacts (Persons)

| Method | Endpoint | Description |
|---|---|---|
| GET | /api/contacts?page=1&size=10 | List contacts (paged) |
| GET | /api/contacts/{id} | Get a contact by ID |
| POST | /api/contacts | Create a contact |
| PUT | /api/contacts/{id} | Update a contact |
| DELETE | /api/contacts/{id} | Delete a contact |
| GET | /api/contacts/{id}/notes | List notes for a contact |
| POST | /api/contacts/{id}/notes | Add a note |
| DELETE | /api/contacts/{id}/notes/{noteId} | Delete a note |

A .http file with ready-to-run requests is available at WebApi/CoreApp.http.

## Roles

| Role | Description |
|---|---|
| Administrator | Full system access |
| SalesManager | Management-level access |
| Salesperson | Default role for self-registered users |
| SupportAgent | Support access |
| ReadOnly | Read-only access |
