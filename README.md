# Waneesy — Child & Mother Care Platform

A graduation project designed to support mothers in following their children's development through interactive educational activities, progress tracking, and a dedicated mother dashboard.

## Project Overview

**Waneesy** is a child and mother care platform that combines an interactive experience for children with tools that help mothers monitor their children's activities and progress.

The platform provides educational content and activities, including stories, games, videos, and daily tasks. It also offers a mother dashboard to manage children's information and review their progress.

This repository contains the **Backend RESTful API** developed using ASP.NET Core Web API and organized around Clean Architecture principles.

## Live Demo

| Component | Link |
|---|---|
| Backend API / Swagger UI | [Waneesy API](https://waneesy.runasp.net/index.html) |
| Mother Dashboard | [Open Dashboard](https://motherdashboard.vercel.app/login) |
| Project Presentation | [View Project Presentation](https://drive.google.com/file/d/1KMwPnIsvK6DHIlMSj-rdC3ok7qwIItBh/view) |

> The deployed links are provided for demonstration purposes. Availability may vary.

## Key Features

### Child Management
- Create and manage child profiles.
- Update personal information and avatar images.
- Associate children with their parent accounts.
- Retrieve child activity summaries and reports.

### Educational Activities
- Stories and story progress tracking.
- Educational games and score tracking.
- Videos and video activity tracking.
- Daily tasks and task completion logs.
- Articles and educational content.

### Parent Dashboard Support
- Parent registration and login.
- Manage children linked to a parent account.
- View children's activities and progress.
- Access weekly reports and activity summaries.
- Manage parent profile information.

### Authentication & Authorization
- JWT Bearer authentication.
- Role-based authorization for Parent, Child, and Admin access.
- Protected API endpoints based on user roles.

### Background Processing
- Hosted background service for daily task-log reset operations.

## Architecture

The backend follows a layered Clean Architecture approach to separate business logic, application workflows, infrastructure concerns, and API delivery.

```text
Waneesy Backend
│
├── kidsApp.Api
│   ├── Controllers
│   ├── API Configuration
│   └── Authentication & Authorization
│
├── kidsApp.Application
│   ├── DTOs
│   ├── Services
│   ├── ServiceManager
│   └── Mapping
│
├── kidsApp.Domain
│   ├── Entities
│   └── Contracts
│
└── kidsApp.Infrastructure
    ├── Data
    ├── Configurations
    ├── Repositories
    ├── UnitOfWork
    └── BackgroundServices
```

### Architectural Responsibilities

| Layer | Responsibility |
|---|---|
| **Domain** | Core entities and contracts that define the business model. |
| **Application** | Application services, DTOs, mapping, and business workflows. |
| **Infrastructure** | Database access, Entity Framework Core, repositories, Unit of Work, and background services. |
| **API** | HTTP endpoints, request handling, authentication, authorization, and API configuration. |

### Request Flow

```text
Client Applications
       │
       ▼
ASP.NET Core API
       │
       ▼
Application Services
       │
       ▼
Domain Contracts
       │
       ▼
Infrastructure
       │
       ▼
SQL Server Database
```

## Design Patterns & Practices

The project applies several software design patterns and development practices:

- **Repository Pattern:** Separates data access logic from application services.
- **Unit of Work:** Coordinates repository operations and database persistence.
- **Service Manager:** Provides centralized access to application services.
- **Dependency Injection:** Manages dependencies across application layers.
- **DTO Pattern:** Separates API request/response models from domain entities.
- **AutoMapper:** Handles object-to-object mapping.
- **Clean Architecture:** Maintains separation of concerns between layers.

## API Modules

The API is organized into versioned controller modules under `/api/v1`.

| Module | Responsibility |
|---|---|
| Auth | Login, token generation, and account-related authentication operations. |
| Parents | Parent profile and child association management. |
| Children | Child profiles, avatars, reports, and activity summaries. |
| Stories | Educational stories management. |
| Story Progress | Tracking story progress. |
| Games | Educational games management. |
| Game Scores | Recording and retrieving game scores. |
| Videos | Educational videos management. |
| Video Activities | Tracking video activities. |
| Tasks | Daily tasks management. |
| Task Logs | Tracking task completion. |
| Articles | Educational articles management. |
| Admin | Administrative operations. |

## Technology Stack

**Backend**
- C#
- .NET 8
- ASP.NET Core Web API

**Database & ORM**
- Microsoft SQL Server
- Entity Framework Core

**Architecture & Development**
- Clean Architecture
- Repository Pattern
- Unit of Work
- Dependency Injection
- DTOs
- AutoMapper

**Authentication & API Documentation**
- JWT Bearer Authentication
- Role-based Authorization
- ASP.NET Core Identity integration
- Swagger / OpenAPI

**Deployment**
- MonsterASP — Backend API hosting
- Vercel — Mother Dashboard hosting

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or another compatible .NET IDE

### 1. Clone the Repository

```bash
git clone https://github.com/Somaya-Mohamed/GP-Waneesy-Backend.git
cd GP-Waneesy-Backend
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure Application Secrets

The application requires a SQL Server connection string and a JWT signing key.

Initialize User Secrets from the API project directory:

```bash
cd kidsApp.Api
dotnet user-secrets init
```

Set the required configuration values:

```bash
dotnet user-secrets set "ConnectionStrings:Connection" "YOUR_CONNECTION_STRING"
dotnet user-secrets set "Jwt:Key" "YOUR_JWT_SIGNING_KEY"
```

The JWT issuer and audience are configured through the application's general settings.

**Never commit real connection strings, passwords, or signing keys to source control.**

### 4. Run the API

```bash
dotnet run
```

The API will start using the configured local launch profile.

## Configuration

The application reads its database and JWT settings through ASP.NET Core configuration.

| Setting | Purpose |
|---|---|
| `ConnectionStrings:Connection` | SQL Server connection string. |
| `Jwt:Key` | JWT signing key. |
| `Jwt:Issuer` | Token issuer. |
| `Jwt:Audience` | Token audience. |

For deployment environments, sensitive configuration should be supplied through environment variables or a secure secrets provider.

## Project Status

Waneesy was developed as an academic graduation project.

The backend is deployed for demonstration and portfolio purposes. The project is not presented as a commercial production service, and it requires further security hardening and review before handling real users or sensitive personal data.

## Contributors

Developed as a collaborative graduation project by the Waneesy team.

**Backend Development:** Somaya Mohamed and the Backend Team.

---

<p align="center">
  <strong>Waneesy</strong><br/>
  Supporting mothers. Empowering children. Making learning meaningful.
</p>
