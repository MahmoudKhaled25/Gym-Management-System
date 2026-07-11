# 🏋️ Gym Management System API

A comprehensive RESTful API for managing gym operations, built with **ASP.NET Core** following **Clean Architecture** principles.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Project Structure](#project-structure)

---

## Overview

Gym Management System is a full-featured backend API that handles all aspects of gym operations including membership management, workout planning, subscription handling, trainer assignments, and real-time notifications.

---

## Architecture

The project follows **Clean Architecture** with **CQRS** pattern using **MediatR**:

```
├── GymManagementSystem.Domain          → Entities, Enums, Errors, Abstractions
├── GymManagementSystem.Application     → CQRS Commands/Queries, Interfaces, Validators
├── GymManagementSystem.Infrastructure  → EF Core, Services, Repositories, JWT
└── GymManagementSystem.Api             → Controllers, Program.cs, DI
```

---

## Features

### 🔐 Authentication & Authorization
- JWT Authentication with Refresh Tokens
- Role-Based Authorization (SuperAdmin, Admin, Trainer, Member)
- Email Confirmation with OTP
- Forget Password / Reset Password via OTP
- Account Lockout after failed login attempts

### 👥 User Management
- Member registration and profile management
- Profile picture upload
- Trainer management and assignment
- Admin management (SuperAdmin only)

### 💳 Membership & Subscriptions
- Multiple membership plans (Basic, Standard, Premium, VIP)
- Subscription request workflow (Member requests → Admin approves/rejects)
- Automatic trainer assignment based on plan type
- Trainer change option for eligible members
- Automatic subscription expiration via background jobs

### 🏋️ Workout Management
- Workout plan creation and assignment
- Exercise library management
- Workout plan exercise tracking (Sets, Reps, Weight, Rest Time)

### 📊 Progress Tracking
- Member progress logs (weight, notes)
- Progress history with pagination and filtering

### 📱 Notifications
- WhatsApp notifications via **Twilio** for subscription reminders
- Email notifications via **Gmail SMTP** (MailKit)
- Bulk offer notifications to all active members

### ⚙️ Background Jobs (Hangfire)
- Auto-expire subscriptions daily
- Send WhatsApp reminders 3 days before subscription expiry
- Hangfire Dashboard with Basic Authentication

### 📈 Admin Dashboard
- Total members, trainers, subscriptions statistics
- Revenue tracking
- Subscriptions by plan chart
- Monthly new members and revenue charts

### 🔧 Technical Features
- Pagination, Sorting & Filtering on all list endpoints
- In-Memory Caching for frequently accessed data
- Rate Limiting (Fixed Window & Sliding Window)
- CORS configuration
- Structured Logging with **Serilog**
- Generic Repository Pattern + Unit of Work

---

## Tech Stack

| Category | Technology |
|---|---|
| Framework | ASP.NET Core (.NET 10) |
| Architecture | Clean Architecture + CQRS |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Authentication | JWT Bearer + ASP.NET Identity |
| Messaging | MediatR |
| Validation | FluentValidation |
| Mapping | Mapster |
| Background Jobs | Hangfire |
| Email | MailKit (Gmail SMTP) |
| WhatsApp | Twilio |
| Logging | Serilog |
| Caching | In-Memory Cache |

---

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server
- Twilio Account (for WhatsApp notifications)
- Gmail Account with App Password (for email)

### Setup

**1. Clone the repository**
```bash
git clone https://github.com/MahmoudKhaled25/Gym-Management-System.git
cd Gym-Management-System
```

**2. Configure User Secrets**
```bash
cd GymManagementSystem.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Your_Connection_String"
dotnet user-secrets set "ConnectionStrings:HangfireConnection" "Your_Hangfire_Connection_String"
dotnet user-secrets set "Jwt:Key" "Your_JWT_Secret_Key"
dotnet user-secrets set "EmailSettings:Password" "Your_Gmail_App_Password"
dotnet user-secrets set "TwilioSettings:AccountSid" "Your_Twilio_SID"
dotnet user-secrets set "TwilioSettings:AuthToken" "Your_Twilio_Auth_Token"
```

**3. Apply Migrations**
```bash
dotnet ef database update --project GymManagementSystem.Infrastructure --startup-project GymManagementSystem.Api
```

**4. Run the API**
```bash
cd GymManagementSystem.Api
dotnet run
```

---

## API Endpoints

### Auth
| Method | Endpoint | Description | Access |
|---|---|---|---|
| POST | `/api/auth/register` | Register new member | Public |
| POST | `/api/auth/login` | Login | Public |
| POST | `/api/auth/refresh-token` | Refresh JWT token | Public |
| POST | `/api/auth/confirm-email` | Confirm email with OTP | Public |
| POST | `/api/auth/resend-confirmation` | Resend confirmation OTP | Public |
| POST | `/api/auth/forget-password` | Request password reset OTP | Public |
| POST | `/api/auth/reset-password` | Reset password with OTP | Public |

### Account
| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/account` | Get current user profile | Authenticated |
| PUT | `/api/account` | Update profile | Authenticated |
| POST | `/api/account/change-password` | Change password | Authenticated |
| POST | `/api/account/profile-image` | Upload profile picture | Authenticated |
| DELETE | `/api/account/profile-image` | Delete profile picture | Authenticated |

### Members
| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/members` | Get all members (paginated) | Admin/SuperAdmin |
| GET | `/api/members/active` | Get active members | Admin/SuperAdmin |
| GET | `/api/members/{id}` | Get member by ID | Admin/SuperAdmin |
| POST | `/api/members` | Add new member | Admin/SuperAdmin |
| PUT | `/api/members/{id}/toggle-status` | Toggle member status | Admin/SuperAdmin |

### Trainers
| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/trainers` | Get all trainers (paginated) | Admin/SuperAdmin |
| GET | `/api/trainers/{id}` | Get trainer by ID | Admin/SuperAdmin |
| POST | `/api/trainers` | Add new trainer | Admin/SuperAdmin |
| PUT | `/api/trainers/{id}` | Update trainer | Admin/SuperAdmin |
| PUT | `/api/trainers/{id}/toggle-status` | Toggle trainer status | Admin/SuperAdmin |
| GET | `/api/trainers/my-members` | Get trainer's members | Trainer |
| GET | `/api/trainers/{id}/members` | Get trainer's members | Admin/SuperAdmin |

### Membership Plans
| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/membershipplans` | Get all plans | Admin/SuperAdmin |
| GET | `/api/membershipplans/active` | Get active plans | Member |
| GET | `/api/membershipplans/{id}` | Get plan by ID | Authenticated |
| POST | `/api/membershipplans` | Create plan | Admin/SuperAdmin |
| PUT | `/api/membershipplans/{id}` | Update plan | Admin/SuperAdmin |
| PUT | `/api/membershipplans/{id}/toggle-status` | Toggle plan status | Admin/SuperAdmin |

### Subscriptions
| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/subscriptions` | Get all subscriptions | Admin/SuperAdmin |
| GET | `/api/subscriptions/{id}` | Get subscription by ID | Admin/SuperAdmin |
| GET | `/api/subscriptions/me` | Get my subscription | Member |
| POST | `/api/subscriptions` | Add subscription | Admin/SuperAdmin |
| PUT | `/api/subscriptions/{id}/cancel` | Cancel subscription | Admin/SuperAdmin |
| PUT | `/api/subscriptions/{id}/change-trainer` | Change trainer | Member |

### Subscription Requests
| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/subscriptionrequests` | Get all requests | Admin/SuperAdmin |
| GET | `/api/subscriptionrequests/me` | Get my requests | Member |
| POST | `/api/subscriptionrequests` | Send subscription request | Member |
| PUT | `/api/subscriptionrequests/{id}/approve` | Approve request | Admin/SuperAdmin |
| PUT | `/api/subscriptionrequests/{id}/reject` | Reject request | Admin/SuperAdmin |

### Workout Plans
| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/workoutplans` | Get all workout plans | Admin/SuperAdmin |
| GET | `/api/workoutplans/{id}` | Get workout plan by ID | Authenticated |
| GET | `/api/workoutplans/me` | Get my workout plans | Member |
| POST | `/api/workoutplans` | Create workout plan | Admin/Trainer |
| PUT | `/api/workoutplans/{id}` | Update workout plan | Admin/Trainer |
| DELETE | `/api/workoutplans/{id}` | Delete workout plan | Admin/Trainer |

### Exercises
| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/exercises` | Get all exercises (paginated) | Authenticated |
| GET | `/api/exercises/{id}` | Get exercise by ID | Authenticated |
| POST | `/api/exercises` | Add exercise | Admin/Trainer |
| PUT | `/api/exercises/{id}` | Update exercise | Admin/Trainer |
| PUT | `/api/exercises/{id}/toggle-status` | Toggle exercise status | Admin |

### Progress Logs
| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/progresslogs` | Get all logs | Admin/Trainer |
| GET | `/api/progresslogs/me` | Get my logs | Member |
| POST | `/api/progresslogs` | Add progress log | Member |
| PUT | `/api/progresslogs/{id}` | Update log | Member |
| DELETE | `/api/progresslogs/{id}` | Delete log | Member |

### Admin
| Method | Endpoint | Description | Access |
|---|---|---|---|
| GET | `/api/admins` | Get all admins | SuperAdmin |
| POST | `/api/admins` | Add new admin | SuperAdmin |
| PUT | `/api/admins/{id}/toggle-status` | Toggle admin status | SuperAdmin |
| GET | `/api/admins/dashboard` | Get dashboard stats | Admin/SuperAdmin |

### Notifications
| Method | Endpoint | Description | Access |
|---|---|---|---|
| POST | `/api/notifications/send-offer` | Send WhatsApp offer to all members | Admin/SuperAdmin |

---

## Project Structure

```
GymManagementSystem.Domain/
├── Entities/          → ApplicationUser, Exercise, Subscription, WorkoutPlan...
├── Enums/             → Gender, SubscriptionStatus, SubscriptionRequestStatus
├── Errors/            → UserErrors, ExerciseErrors, SubscriptionErrors...
├── Abstractions/      → Result, Error, PaginatedList
└── Repositories/      → IRepository<T>, IUnitOfWork, IMemberRepository...

GymManagementSystem.Application/
├── Auth/              → Login, Register, ConfirmEmail, ForgetPassword...
├── Accounts/          → GetProfile, UpdateProfile, UploadImage...
├── Members/           → GetAllMembers, AddMember, ToggleStatus...
├── Trainers/          → GetAllTrainers, AddTrainer...
├── Exercises/         → GetAllExercises, AddExercise, UpdateExercise...
├── Subscriptions/     → AddSubscription, CancelSubscription...
├── WorkoutPlans/      → AddWorkoutPlan, GetMyPlan...
├── ProgressLogs/      → AddLog, GetMyLogs...
├── Admins/            → AddAdmin, GetDashboard...
├── Interfaces/        → IEmailService, INotificationService, IJwtProvider...
└── Common/            → RequestFilters, Behaviors, FileValidators...

GymManagementSystem.Infrastructure/
├── Persistence/       → ApplicationDbContext, Configurations, Migrations
├── Repositories/      → Repository<T>, UnitOfWork, MemberRepository...
├── Queries/           → ExerciseQueries, MemberQueries, TrainerQueries...
├── Authentication/    → JwtProvider, JwtOptions
├── Services/          → EmailService, NotificationService, FileStorageService...
├── Jobs/              → SubscriptionJobService, NotificationJobService
└── Settings/          → EmailSettings, TwilioSettings, HangfireSettings

GymManagementSystem.Api/
├── Controllers/       → AuthController, MembersController, ExercisesController...
└── Program.cs
```

---

## Default Credentials

After running migrations, a default SuperAdmin account is created:

| Field | Value |
|---|---|
| Email | Admin@Gym.com |
| Password | P@ssword123 |

> ⚠️ Please change the default password after first login.

---

## Author

**Mahmoud Khaled**  
GitHub: [@MahmoudKhaled25](https://github.com/MahmoudKhaled25)

---

*Built with ❤️ using ASP.NET Core Clean Architecture*
