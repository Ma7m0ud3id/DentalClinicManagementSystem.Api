# 🦷 Dental Clinic Management System

A comprehensive backend REST API for managing dental clinic operations, built with **.NET 10** following a clean **3-Tier Architecture**. The system handles staff management, patient records, appointment scheduling with overlap prevention, visit documentation, prescriptions tracking, and role-based dashboard analytics.

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Project Structure](#-project-structure)
- [Database Schema](#-database-schema)
- [User Roles & Permissions](#-user-roles--permissions)
- [Getting Started](#-getting-started)
- [Default Users](#-default-users)
- [API Endpoints](#-api-endpoints)
- [Business Rules](#-business-rules)
- [Authentication Flow](#-authentication-flow)
- [Testing Scenarios](#-testing-scenarios)
- [Future Roadmap](#-future-roadmap)

---

## 🎯 Overview

The Dental Clinic Management System is designed to streamline the daily operations of a dental clinic, supporting multiple doctors, receptionists, administrators, and patient self-service. The system enforces strict business rules to prevent scheduling conflicts and maintain data integrity.

### Key Highlights
- 🏥 **Multi-User System** with role-based access control
- 🔐 **JWT Authentication** for both staff and patients
- 📅 **Smart Appointment Scheduling** with overlap prevention
- 👤 **Patient Self-Registration** with phone-based account merging
- 📊 **Real-time Dashboard** with role-filtered statistics
- 🗃️ **Soft Delete** pattern for patient records
- 📖 **Interactive API Documentation** via Scalar

---

## ✨ Features

### Core Modules

| Module | Description |
|--------|-------------|
| 🔐 **Authentication** | JWT-based login for staff and patients with role-based authorization |
| 👨‍⚕️ **Doctors Management** | Admin manages doctors (CRUD + activation toggle) |
| 👥 **Patient Records** | Full CRUD with soft delete, search, and optional account creation |
| 📅 **Appointments** | Schedule appointments with automatic conflict detection |
| 📝 **Visits** | Document patient visits with auto-completion of appointments |
| 📊 **Dashboard** | Role-filtered statistics for quick insights |
| 🛠️ **User Management** | Admin manages all system users (Admin/Doctor/Receptionist) |
| 🌐 **Patient Portal** | Patients can register, login, and view their own data |

### Technical Features
- ✅ Clean **3-Tier Architecture** (API → BLL → DAL)
- ✅ **JWT Bearer Authentication** with role + userType claims
- ✅ **FluentValidation** for robust input validation
- ✅ **Global Exception Handling** middleware
- ✅ **Auto Database Migration** on startup
- ✅ **Auto Data Seeding** on first run
- ✅ **Scalar API Documentation** with Bearer token support
- ✅ **BCrypt Password Hashing**
- ✅ **Soft Delete** for patient records
- ✅ **Manual Mapping** (no AutoMapper overhead)
- ✅ **Custom Authorization Attributes** for fine-grained access control

---

## 🛠 Tech Stack

| Category | Technology |
|----------|-----------|
| **Framework** | .NET 10 |
| **API** | ASP.NET Core Web API |
| **ORM** | Entity Framework Core |
| **Database** | SQL Server |
| **Authentication** | JWT Bearer Tokens |
| **Validation** | FluentValidation |
| **Password Hashing** | BCrypt.Net-Next |
| **API Documentation** | Scalar (OpenAPI) |
| **Architecture** | 3-Tier (API / BLL / DAL) |

---

## 🏗 Architecture

The project follows a strict **3-Tier Architecture** with clear separation of concerns:

```text
┌──────────────────────────────────────────────┐
│  DentalClinicManagementSystem.Apis           │  ← Presentation Layer
│  • Controllers                                │
│  • Middleware                                 │
│  • Custom Authorization Attributes            │
│  • Program.cs                                 │
└─────────────────┬────────────────────────────┘
                  │ references
                  ▼
┌──────────────────────────────────────────────┐
│  DentalClinicManagementSystem.BLL            │  ← Business Logic Layer
│  • Services (Interfaces + Implementations)   │
│  • DTOs                                       │
│  • Validators (FluentValidation)              │
│  • JWT Token Generation                       │
│  • Current User Service                       │
└─────────────────┬────────────────────────────┘
                  │ references
                  ▼
┌──────────────────────────────────────────────┐
│  DentalClinicManagementSystem.DAL            │  ← Data Access Layer
│  • Entities                                   │
│  • Enums                                      │
│  • DbContext                                  │
│  • EF Core Configurations                     │
│  • Database Seeder                            │
└──────────────────────────────────────────────┘
```

### Architectural Decisions

| Decision | Reasoning |
|----------|-----------|
| ❌ No Repository Pattern | Services use `AppDbContext` directly — simpler and EF Core already implements it |
| ❌ No AutoMapper | Manual mapping for transparency and control |
| ❌ No CQRS/MediatR | Direct service calls keep things simple for this scope |
| ✅ Service Layer | All business logic in services, controllers stay thin |
| ✅ DI Extensions | Each layer registers its own dependencies |

---

## 📁 Project Structure

```text
DentalClinicManagementSystem/
│
├── DentalClinicManagementSystem.Apis/        ← API Layer
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── PatientAuthController.cs
│   │   ├── UsersController.cs
│   │   ├── DoctorsController.cs
│   │   ├── PatientsController.cs
│   │   ├── AppointmentsController.cs
│   │   ├── VisitsController.cs
│   │   └── DashboardController.cs
│   ├── Authorization/
│   │   └── PatientOnlyAttribute.cs
│   ├── Middleware/
│   │   └── ExceptionHandlingMiddleware.cs
│   ├── Extensions/
│   │   ├── OpenApiExtensions.cs
│   │   └── DatabaseInitializerExtensions.cs
│   ├── Program.cs
│   └── appsettings.json
│
├── DentalClinicManagementSystem.BLL/         ← Business Logic Layer
│   ├── DTOs/
│   │   ├── Auth/
│   │   ├── Users/
│   │   ├── Doctors/
│   │   ├── Patients/
│   │   ├── Patient/      (patient-facing DTOs)
│   │   ├── Appointments/
│   │   ├── Visits/
│   │   └── Dashboard/
│   ├── Services/
│   │   ├── Interfaces/
│   │   └── Implementations/
│   ├── Validators/
│   ├── Security/
│   │   ├── JwtSettings.cs
│   │   └── JwtTokenGenerator.cs
│   ├── CurrentUser/
│   │   ├── ICurrentUserService.cs
│   │   └── CurrentUserService.cs
│   └── Extensions/
│       └── BusinessLayerExtensions.cs
│
└── DentalClinicManagementSystem.DAL/         ← Data Access Layer
    ├── Entities/
    │   ├── User.cs
    │   ├── Patient.cs
    │   ├── Appointment.cs
    │   └── Visit.cs
    ├── Enums/
    │   ├── UserRole.cs
    │   ├── Gender.cs
    │   └── AppointmentStatus.cs
    ├── Data/
    │   └── AppDbContext.cs
    ├── Configurations/
    │   ├── UserConfiguration.cs
    │   ├── PatientConfiguration.cs
    │   ├── AppointmentConfiguration.cs
    │   └── VisitConfiguration.cs
    ├── Seed/
    │   └── DatabaseSeeder.cs
    └── Extensions/
        └── DataAccessExtensions.cs
```

---

## 🗄 Database Schema

### Tables Overview

#### 👤 Users
Stores all staff members (Admin, Doctor, Receptionist) in a single table using a role discriminator.

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | |
| Username | nvarchar(50) | Unique, Required | |
| PasswordHash | nvarchar(500) | Required | BCrypt hash |
| FullName | nvarchar(100) | Required | |
| Phone | nvarchar(20) | Unique, Required | |
| Role | int | Required | 1=Admin, 2=Doctor, 3=Receptionist |
| Specialization | nvarchar(100) | Nullable | Required only for Doctors |
| IsActive | bit | Default: true | |
| CreatedAt | datetime2 | Required | UTC |

#### 👥 Patients
Stores patient records with optional account credentials for self-service.

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | |
| Username | nvarchar(50) | Unique (filtered), Nullable | For patients with accounts |
| PasswordHash | nvarchar(500) | Nullable | BCrypt hash |
| Email | nvarchar(100) | Nullable | |
| FullName | nvarchar(100) | Required | |
| Phone | nvarchar(20) | Unique (filtered) | Among non-deleted |
| DateOfBirth | date | Nullable | |
| Gender | int | Required | 1=Male, 2=Female |
| Address | nvarchar(200) | Nullable | |
| MedicalNotes | nvarchar(1000) | Nullable | |
| IsDeleted | bit | Default: false | Soft delete flag |
| CreatedAt | datetime2 | Required | UTC |

#### 📅 Appointments
Records of scheduled appointments with conflict prevention.

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | |
| PatientId | int | FK → Patients | |
| DoctorId | int | FK → Users | Must have Role=Doctor |
| AppointmentDate | datetime2 | Required | |
| DurationMinutes | int | Default: 30 | |
| Status | int | Required | 1=Scheduled, 2=Completed, 3=Cancelled |
| Notes | nvarchar(500) | Nullable | |
| CreatedAt | datetime2 | Required | UTC |

#### 📝 Visits
Documentation of completed patient visits.

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | |
| AppointmentId | int | FK → Appointments, Unique | One visit per appointment |
| PatientId | int | FK → Patients | Auto-derived |
| DoctorId | int | FK → Users | Auto-derived |
| Diagnosis | nvarchar(500) | Required | |
| Treatment | nvarchar(500) | Required | |
| DoctorNotes | nvarchar(1000) | Nullable | |
| VisitDate | datetime2 | Required | |
| CreatedAt | datetime2 | Required | UTC |

### Entity Relationships

```text
Users (Doctor) ──┬─< Appointments
                 └─< Visits

Patients ────────┬─< Appointments
                 └─< Visits

Appointments 1───1 Visits  (one-to-one optional)
```

---

## 👥 User Roles & Permissions

### Staff Roles

| Role | Capabilities |
|------|--------------|
| 🔴 **Admin** | Full system access. Manage all users, doctors, patients, appointments, and view all data |
| 🟢 **Doctor** | View own appointments, create/update visits for own patients, view patient records |
| 🔵 **Receptionist** | Manage patients (CRUD), manage appointments (CRUD), view doctors list |

### Patient Role

| Role | Capabilities |
|------|--------------|
| 🟡 **Patient** | Self-register, login, view own profile, view own appointments, view own visits |

### Permission Matrix

| Operation | Admin | Doctor | Receptionist | Patient |
|-----------|:-----:|:------:|:------------:|:-------:|
| Create User | ✅ | ❌ | ❌ | ❌ |
| Create Doctor | ✅ | ❌ | ❌ | ❌ |
| Create Patient | ✅ | ❌ | ✅ | Self only |
| Update Patient | ✅ | ❌ | ✅ | Self only |
| Delete Patient | ✅ | ❌ | ✅ | ❌ |
| Create Appointment | ✅ | ❌ | ✅ | (Step 13) |
| Cancel Appointment | ✅ | ❌ | ✅ | (Step 13) |
| Create Visit | ✅ | ✅ (own) | ❌ | ❌ |
| View Dashboard | ✅ All | ✅ Filtered | ✅ All | ❌ |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express, or full version)
- IDE: Visual Studio 2022, VS Code, or JetBrains Rider

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/YOUR_USERNAME/dental-clinic-management.git
   cd dental-clinic-management
   ```

2. **Update connection string**

   Edit `DentalClinicManagementSystem.Apis/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=DentalClinicDb;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

3. **Install EF Core CLI tools** (if not already installed)
   ```bash
   dotnet tool install --global dotnet-ef
   ```

4. **Build the solution**
   ```bash
   dotnet build
   ```

5. **Apply database migrations** (optional — runs automatically on startup)
   ```bash
   dotnet ef database update --project DentalClinicManagementSystem.DAL --startup-project DentalClinicManagementSystem.Apis
   ```

6. **Run the API**
   ```bash
   dotnet run --project DentalClinicManagementSystem.Apis
   ```

   On first run, the system will:
   - ✅ Apply all migrations
   - ✅ Seed default users

7. **Open API Documentation**

   Navigate to: `https://localhost:{port}/scalar/v1`

---

## 🔑 Default Users

The database is automatically seeded with these credentials on first run:

| Username | Password | Role | Description |
|----------|----------|------|-------------|
| `admin` | `Admin@123` | Admin | Full system administrator |
| `doctor1` | `Doctor@123` | Doctor | General Dentist |
| `recep1` | `Recep@123` | Receptionist | Front desk staff |

> ⚠️ **Security Notice:** Change these passwords before deploying to production!

---

## 📡 API Endpoints

### 🔐 Authentication
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `/api/auth/login` | Public | Staff login (returns JWT) |
| `GET` | `/api/auth/me` | Authenticated | Get current logged-in user |
| `POST` | `/api/patient/auth/register` | Public | Patient self-registration |
| `POST` | `/api/patient/auth/login` | Public | Patient login |

### 🛠️ User Management (Admin Only)
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/users?role=&isActive=` | List all users (filterable) |
| `GET` | `/api/users/{id}` | Get user by ID |
| `POST` | `/api/users` | Create any type of user |
| `PUT` | `/api/users/{id}` | Update user details |
| `PATCH` | `/api/users/{id}/toggle-status` | Activate/deactivate user |
| `PATCH` | `/api/users/{id}/change-password` | Reset user password |

### 👨‍⚕️ Doctors
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `/api/doctors` | Any authenticated | List all doctors |
| `GET` | `/api/doctors/{id}` | Any authenticated | Get doctor by ID |
| `POST` | `/api/doctors` | Admin | Create doctor |
| `PUT` | `/api/doctors/{id}` | Admin | Update doctor |
| `PATCH` | `/api/doctors/{id}/toggle-status` | Admin | Activate/deactivate |

### 👥 Patients
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `/api/patients?search=` | Any authenticated | List/search patients |
| `GET` | `/api/patients/{id}` | Any authenticated | Get patient details |
| `POST` | `/api/patients` | Admin, Receptionist | Create patient (with optional account) |
| `PUT` | `/api/patients/{id}` | Admin, Receptionist | Update patient |
| `DELETE` | `/api/patients/{id}` | Admin, Receptionist | Soft delete |

### 📅 Appointments
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `/api/appointments?date=&doctorId=&status=` | Any authenticated | List (filterable) |
| `GET` | `/api/appointments/today` | Any authenticated | Today's appointments |
| `GET` | `/api/appointments/{id}` | Any authenticated | Get by ID |
| `POST` | `/api/appointments` | Admin, Receptionist | Create appointment |
| `PUT` | `/api/appointments/{id}` | Admin, Receptionist | Update appointment |
| `PATCH` | `/api/appointments/{id}/cancel` | Admin, Receptionist | Cancel appointment |
| `PATCH` | `/api/appointments/{id}/complete` | Admin, Receptionist | Mark completed |

### 📝 Visits
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `/api/visits` | Any authenticated | List visits |
| `GET` | `/api/visits/{id}` | Any authenticated | Get visit by ID |
| `GET` | `/api/visits/patient/{patientId}` | Any authenticated | Patient's visit history |
| `POST` | `/api/visits` | Admin, Doctor | Create visit |
| `PUT` | `/api/visits/{id}` | Admin, Doctor | Update visit |

### 📊 Dashboard
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `/api/dashboard/stats` | Any authenticated | Get statistics (role-filtered) |

---

## 📜 Business Rules

### 🔐 Authentication
- Passwords are hashed using **BCrypt**
- JWT tokens expire after **60 minutes**
- Inactive accounts cannot log in
- Staff and Patients use **separate authentication flows**
- JWT includes a `userType` claim (`"Staff"` or `"Patient"`)

### 👨‍⚕️ Doctors
- Stored in `Users` table with `Role = Doctor`
- Username and Phone must be **unique** across all users
- **Inactive doctors** cannot receive new appointments
- **No hard delete** — only toggle active status
- Specialization is **required** for doctors

### 👥 Patients
- Phone number must be **unique** among non-deleted patients
- **Soft delete only** (`IsDeleted = true`)
- Search supports partial match on **name** and **phone**
- Optional account creation (Username + Password)
- Auto-merge with existing record by phone during self-registration

### 📅 Appointments
- ⛔ Cannot be created **in the past**
- ⛔ Cannot **overlap** with existing appointments for the same doctor
- **Overlap rule:** `newStart < existingEnd AND newEnd > existingStart`
- Default duration: **30 minutes**
- Cannot modify **cancelled** or **completed** appointments
- Cannot **cancel** completed appointments
- Cannot **complete** cancelled appointments
- Doctors see **only their own** appointments

### 📝 Visits
- **One visit per appointment** (enforced by unique index)
- Cannot create visit for **cancelled** appointment
- `PatientId` and `DoctorId` are **auto-derived** from the appointment
- Creating a visit **automatically marks** the appointment as `Completed`
- Doctors can only create/edit **their own** visits

### 🛠️ User Management
- Admin **cannot deactivate** their own account
- Specialization is required **only for doctors**
- Username and Phone are unique across **all users**

---

## 🔄 Authentication Flow

### Staff Login Flow
```text
1. POST /api/auth/login
   ↓
2. Server validates credentials
   ↓
3. Returns JWT with claims:
   • NameIdentifier (UserId)
   • Name (FullName)
   • Role (Admin/Doctor/Receptionist)
   • username
   • userType = "Staff"
   ↓
4. Client stores token
   ↓
5. Sends "Authorization: Bearer {token}" with each request
```

### Patient Registration Flow
```text
1. POST /api/patient/auth/register
   ↓
2. Server checks for existing patient by phone
   ↓
3a. If exists & no account → MERGE (link new account to existing record)
3b. If exists & has account → REJECT (409 Conflict)
3c. If not exists → CREATE new patient record
   ↓
4. Returns JWT with userType = "Patient"
```

---

## 🧪 Testing Scenarios

### Scenario 1: Complete Patient Journey
```http
# Step 1: Receptionist creates a patient
POST /api/auth/login
{ "username": "recep1", "password": "Recep@123" }

POST /api/patients
{
  "fullName": "Ahmed Mohamed",
  "phone": "01012345678",
  "gender": 1,
  "address": "Cairo"
}

# Step 2: Receptionist books an appointment
POST /api/appointments
{
  "patientId": 1,
  "doctorId": 2,
  "appointmentDate": "2025-02-20T10:00:00",
  "durationMinutes": 30,
  "notes": "Routine checkup"
}

# Step 3: Doctor logs in and records the visit
POST /api/auth/login
{ "username": "doctor1", "password": "Doctor@123" }

POST /api/visits
{
  "appointmentId": 1,
  "diagnosis": "Tooth decay in upper molar",
  "treatment": "Composite filling",
  "doctorNotes": "Follow-up in 6 months",
  "visitDate": "2025-02-20T10:00:00"
}

# Step 4: Admin checks dashboard
POST /api/auth/login
{ "username": "admin", "password": "Admin@123" }

GET /api/dashboard/stats
```

### Scenario 2: Patient Self-Registration with Merge
```http
# A patient with phone 01055554444 already exists (created by receptionist)
# Patient registers with the same phone

POST /api/patient/auth/register
{
  "username": "ahmed_p",
  "password": "Pass@123",
  "fullName": "Ahmed Mohamed",
  "phone": "01055554444",
  "gender": 1
}

# Result: Account is linked to existing patient record (not duplicated)
```

### Scenario 3: Overlap Prevention
```http
# Existing appointment: Doctor 2, 10:00-10:30

POST /api/appointments
{
  "patientId": 1,
  "doctorId": 2,
  "appointmentDate": "2025-02-20T10:15:00",
  "durationMinutes": 30
}

# Returns: 400 Bad Request - "Doctor has an overlapping appointment"
```

---

## 🔮 Future Roadmap

### Phase 2 - Patient Self-Service Enhancement
- 📋 Patient profile management (view + update)
- 📅 Patient views own appointments
- 📝 Patient views own visit history

### Phase 3 - Patient Booking
- 🗓️ Patient books appointments directly
- ❌ Patient cancels appointments (24-hour rule)
- 🔍 View available time slots per doctor

### Phase 4 - Communication
- 📱 WhatsApp appointment reminders
- 📧 Email notifications
- 🔔 In-app notifications

### Phase 5 - Financial
- 💰 Invoicing & billing system
- 💳 Online payments (Paymob/Fawry)
- 📊 Financial reports

### Phase 6 - Advanced
- 💊 Prescriptions module
- 📅 Google Calendar sync
- 📱 Mobile app (Flutter / .NET MAUI)
- 📊 Advanced analytics with charts
- 🌐 Multi-branch support

---

## 🔒 Security Considerations

- ✅ Passwords hashed with **BCrypt** (industry standard)
- ✅ JWT tokens with **short expiration** (60 min)
- ✅ **Role-based** authorization on all endpoints
- ✅ **Separate authentication** for staff and patients
- ✅ **Input validation** on all DTOs via FluentValidation
- ✅ **Soft delete** to preserve data integrity
- ⚠️ Use **HTTPS** in production
- ⚠️ Store **JWT secret** in environment variables or Azure Key Vault for production
- ⚠️ Change **default passwords** before deployment

---

## 📦 Deployment Checklist

Before deploying to production:

- [ ] Change all default passwords
- [ ] Use a strong, random JWT secret (32+ characters)
- [ ] Move secrets to environment variables or Azure Key Vault
- [ ] Configure CORS for your frontend domain
- [ ] Enable HTTPS only
- [ ] Set up proper logging (Serilog / Application Insights)
- [ ] Configure SQL Server connection with proper credentials
- [ ] Set `ASPNETCORE_ENVIRONMENT=Production`
- [ ] Review and tighten all CORS policies
- [ ] Set up automated backups for the database

---

## 🤝 Contributing

This is a portfolio/learning project. Feel free to fork, modify, and use it for your own learning purposes.

---

## 📄 License

MIT License - Free for personal and educational use.

---

## 👨‍💻 Author

Built as a comprehensive portfolio project demonstrating:
- ✨ Clean **3-Tier Architecture** in .NET
- ✨ **RESTful API** design principles
- ✨ **JWT Authentication** with multiple user types
- ✨ **Role-Based Authorization**
- ✨ **Domain-Driven Business Rules**
- ✨ **Entity Framework Core** best practices
- ✨ **FluentValidation** for input validation
- ✨ Modern **API documentation** with Scalar

---

## 🙏 Acknowledgments

Built with the assistance of:
- **GitHub Copilot** (Claude 3.5 Sonnet) for code generation
- **.NET Community** for excellent documentation
- **Scalar** for beautiful API documentation

---

**⭐ If you find this project helpful, please consider giving it a star on GitHub!**

---

## 📞 Support

For questions or issues, please open an issue on GitHub or contact the maintainer.

**Happy Coding! 🚀**
