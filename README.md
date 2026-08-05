# Gym Management System

A gym management web application built with **ASP.NET Core MVC**, following a layered architecture (Presentation → Business Logic → Data Access) with the Repository / Unit of Work pattern, AutoMapper for object mapping, and ASP.NET Core Identity for authentication and role-based access.

> Status: mostly complete — core modules are functional, with ongoing polishing and refinement.

## Overview

The system manages the day-to-day operations of a gym, including trainers, members, membership plans, and scheduled training sessions, with role-based access for Admins, Trainers, and Members.

## Features

- **Trainer management** — add, view, edit, and delete trainers, including specialities and contact details
- **Member management** — member registration with profile photo upload, address, and health record (height, weight, blood type, notes)
- **Session scheduling** — create and manage gym sessions, linking a trainer and category, with capacity and available-slot tracking
- **Membership plans** — manage gym plans and member subscriptions
- **File uploads** — profile photo upload/storage/retrieval for members, served back through a dedicated attachment service
- **Role-based access** — Admin, Trainer, and Member roles via ASP.NET Core Identity (`RoleManager<IdentityRole>`)
- **Validation** — server-side model validation with user-friendly error messages across all forms

## Architecture

The solution is organized into three layers:

```
GymManagement.PL   → Presentation Layer (ASP.NET Core MVC: Controllers, Views)
GymManagement.BLL  → Business Logic Layer (Services, ViewModels, AutoMapper profiles)
GymManagement.DAL  → Data Access Layer (EF Core DbContext, Entities, Repositories)
```

### Key design patterns

- **Repository + Unit of Work** — a generic repository (`IGenericRepository<T>`) handles common CRUD operations, with entity-specific repositories (e.g. `ISessionRepository`, `IPlanRepository`) extending it for specialized queries (eager-loaded includes, custom filters). A `IUnitRepository` coordinates repository instances and commits changes as a single unit.
- **Service layer** — business rules, validation, and orchestration live in services (`ITrainerService`, `IMemberService`, `ISessionService`, etc.), keeping controllers thin and repositories free of business logic.
- **Result pattern** — service methods return a `Result` / `Result<T>` object (success flag, optional model, message, and result kind) instead of throwing exceptions for expected failure cases like validation errors or "not found," giving controllers a consistent way to branch on outcomes.
- **AutoMapper** — maps between entities and ViewModels (e.g. `Session` ↔ `SessionViewModel`/`AddSessionViewModel`/`EditSessionViewModel`), keeping the layers decoupled from each other's shapes.
- **Dependency Injection** — all services, repositories, and the `DbContext` are registered in `Program.cs` and injected via constructors throughout, keeping components loosely coupled and testable.

### Domain model highlights

- `GymUser` — an abstract base class shared by `Member` and `Trainer`, holding common properties (name, email, phone, date of birth, gender, address)
- `Session` — a scheduled class, linked to a `Trainer` and `Category`, with capacity and booked-member tracking
- `Address` / `HealthRecord` — owned/related types capturing structured member details

## Tech stack

- ASP.NET Core MVC (.NET 9)
- Entity Framework Core (SQL Server)
- AutoMapper
- ASP.NET Core Identity
- Bootstrap (UI)

## Project structure

```
GymManagement.PL/
├── Controllers/
├── Views/
└── wwwroot/

GymManagement.BLL/
├── Services/
│   ├── Interfaces/
│   └── Classes/
├── ViewModels/
└── MappingProfile.cs

GymManagement.DAL/
├── Models/
├── Repositories/
│   ├── Interfaces/
│   └── Classes/
└── Migrations/
```

## Notes

This project was built as a hands-on learning exercise in ASP.NET Core MVC, EF Core, and layered application architecture, with an emphasis on clean separation of concerns between controllers, services, and data access.
