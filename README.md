# Gym Management System

A gym management web application built with **ASP.NET Core MVC (.NET 9)**, using a three-layer architecture (Presentation / Business Logic / Data Access), the Repository + Unit of Work pattern, AutoMapper, and ASP.NET Core Identity for authentication and role-based authorization.

> Status: mostly complete — core modules are functional, with ongoing polishing.

## Overview

The system manages the day-to-day operations of a gym: trainers, members (with profile photos and health records), membership plans, and scheduled training sessions. Access is restricted by role, with a dashboard summarizing key gym statistics.

## Features

- **Authentication & authorization** — cookie-based login via ASP.NET Core Identity, with `SuperAdmin` and `Admin` roles; most controllers are `[Authorize]`-protected, with member and trainer management restricted to `SuperAdmin`
- **Dashboard (Home)** — live counts of total/active members, total trainers, and upcoming/ongoing/completed sessions
- **Trainer management** — add, view, edit, and delete trainers, with specialities, contact details, and a check preventing deletion while a trainer has active sessions
- **Member management** — registration with profile photo upload, address, and a linked health record (height, weight, blood type, notes); membership plan and dates shown on the member's details page
- **Session scheduling** — create, view, edit, and delete gym sessions; each session is linked to a trainer and category, with capacity, booked-member counts, and computed status (Upcoming / Ongoing / Completed)
- **Membership plans** — manage plans (duration, price, active/inactive toggle)
- **File uploads** — member profile photos are validated (size/extension), stored outside `wwwroot`, and served back through a dedicated controller endpoint (not a direct static URL)
- **Data seeding** — plan data seeded from a JSON file on startup; default `SuperAdmin`/`Admin` roles and accounts seeded via Identity
- **Server-side validation** — data annotations across all forms, including Egyptian phone number format, with user-facing error messages

## Architecture

```
GymManagement.PL   → Presentation Layer (ASP.NET Core MVC: Controllers, Views) — project/namespace "Gym_Management"
GymManagement.BLL  → Business Logic Layer (Services, ViewModels, AutoMapper profiles, Result type)
GymManagement.DAL  → Data Access Layer (EF Core DbContext, Entities, Configurations, Repositories, Migrations)
```

### Key design patterns

- **Repository + Unit of Work** — `IGenericRepository<T>` provides common CRUD (`GetAllAsync`, `GetByIDAsync`, `AddAsync`, `UpdateAsync`, `DeleteAsync`, `AnyAsync`, `FirstOrDefaultAsync`, `CountAsync`) for any entity deriving from `Base`. `IUnitRepository` hands out and caches repository instances per entity type and commits all changes via a single `SaveChangesAsync`. Entities needing specialized queries (eager-loaded includes, custom joins) get their own repository — e.g. `ISessionRepository` (sessions with trainer/category included), `IPlanRepository`.
- **Service layer** — business rules and orchestration live in services (`ITrainerService`, `IMemberService`, `ISessionService`, `IHomeService`), keeping controllers thin.
- **Result pattern** — most service methods return a `Result` / `Result<T>` (success flag, optional model, message, and a `ResultKind`: Ok, Fail, ValidationFail, NotFound, Forbidden, Conflict) instead of throwing exceptions for expected outcomes, giving controllers a consistent way to branch and surface messages via `TempData`.
- **AutoMapper** — maps entities to/from ViewModels (e.g. `Session` ↔ `SessionViewModel` / `AddSessionViewModel` / `EditSessionViewModel`), keeping layers decoupled from each other's shapes.
- **Fluent API configuration** — `IEntityTypeConfiguration<T>` classes per entity (`SessionConfiguration`, `PlanConfiguration`, etc.), including a shared generic `UserConfiguration<T>` reused by both `Member` and `Trainer` configurations for common constraints (unique name/phone, owned `Address`).
- **Dependency Injection** — all services, repositories, `DbContext`, and Identity managers are registered in `Program.cs` and injected via constructors throughout.

### Domain model

- `Base` — common `ID`, `CreatedAt`, `UpdatedAt` fields shared by most entities
- `User` (abstract) — shared identity fields for `Member` and `Trainer`: `Name`, `Phone`, `Email`, `DateOfBirth`, `Gender`, `Address`
  - `Member` — adds `Photo`, a one-to-one `HealthRecord`, session bookings (`Book`), and plan subscriptions (`MemberPlan`)
  - `Trainer` — adds `Speciality` and their scheduled `Session`s
- `Address` — owned type (building number, street, city)
- `HealthRecord` — height, weight, blood type, notes; one per member
- `Category` — session category (Cardio, Strength, Yoga, Boxing, Nutrition — seeded), tied to a matching `Speciality`
- `Session` — a scheduled class, linked to a `Trainer` and `Category`, with `Capacity`, `StartDate`/`EndDate`, and booked members via `Book`
- `Book` — join entity between `Member` and `Session` (composite key), tracks booking and attendance
- `Plan` — a membership plan (name, description, duration, price, active flag)
- `MemberPlan` — join entity between `Member` and `Plan`, tracking subscription start/end dates

### Database constraints

- Unique indexes on `Member`/`Trainer` `Name` and `Phone`
- Check constraints: session capacity between 1–25, session end date after start date, plan duration between 1–365 days

## Tech stack

- ASP.NET Core MVC (.NET 9)
- Entity Framework Core (SQL Server)
- ASP.NET Core Identity (cookie authentication, role-based authorization)
- AutoMapper
- Bootstrap (UI)

## Project structure

```
GymManagement.PL/ (Gym_Management)
├── Controllers/        (Account, Home, Member, Plans, Session, Trainer)
├── Views/
└── wwwroot/

GymManagement.BLL/
├── Services/
│   ├── Interfaces/
│   ├── Classes/
│   └── AttachmentService/
├── ViewModels/
├── MappingProfile.cs
└── Result.cs

GymManagement.DAL/
├── Models/              (entities + Enum/)
├── Configurations/       (Fluent API IEntityTypeConfiguration<T>)
├── Repositories/
│   ├── Interfaces/
│   └── Classes/
├── Migrations/
├── ApplicationUser.cs    (extends IdentityUser)
├── GybDbContext.cs       (extends IdentityDbContext<ApplicationUser>)
├── IdentityDataSeed.cs
└── DataSeeding.cs
```

## Notes

This project was built as a hands-on learning exercise in ASP.NET Core MVC, EF Core, and layered application architecture, with an emphasis on clean separation of concerns between controllers, services, and data access, and consistent handling of expected failures via the `Result` pattern.
