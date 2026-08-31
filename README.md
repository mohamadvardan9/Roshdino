# Roshdino | Digital Marketing Corporate Website

Roshdino is a corporate showcase website built for a digital marketing business to present its services, products, and content.

The project is built with **ASP.NET Core MVC** and follows a **Layered Architecture** with two independent applications: a public-facing website and a secure administration panel for content management.

---

## 📋 Table of Contents

* [About the Project](#-about-the-project)
* [Features](#-features)
* [Tech Stack](#-tech-stack)
* [Architecture](#-architecture)
* [Project Structure](#-project-structure)
* [Prerequisites](#-prerequisites)
* [Getting Started](#-getting-started)
* [Running Tests](#-running-tests)
* [Security](#-security)
* [Roadmap](#-roadmap)
* [License](#-license)

---

## 📖 About the Project

**Roshdino** is a corporate showcase website designed to present the products and services of a digital marketing business.

The platform does not provide public user registration or account management. Instead, a single **Super Admin** manages website content through a dedicated and secured administration panel.

The public website is focused on content presentation, while the Admin application provides the tools required to manage products, articles, categories, and contact messages.

---

## ✨ Features

### 🌐 Public Website

* Dynamic homepage with Hero, statistics, services, portfolio, and customer testimonials
* Product and article listing
* Category-based content filtering
* SEO-friendly slug-based URLs
* Contact form with Honeypot spam protection
* Dynamic meta tags
* Open Graph metadata
* Automatic sitemap generation
* `robots.txt`
* Fully responsive design
* Custom CSS without a CSS framework
* Vanilla JavaScript for client-side interactions

### 🔐 Admin Panel

* Cookie-based authentication
* Secure password hashing with BCrypt
* Dashboard with content statistics
* Product management
* Article management
* Category management
* Product image gallery management
* Primary image selection
* Rich text editing with Quill.js
* Contact message management
* Admin password change functionality
* Secure-by-default authorization

---

## 🛠 Tech Stack

| Category                | Technology                                   |
| ----------------------- | -------------------------------------------- |
| **Framework**           | ASP.NET Core MVC (.NET 8 LTS)                |
| **Language**            | C#                                           |
| **Database**            | SQL Server                                   |
| **ORM**                 | Entity Framework Core (Code First)           |
| **Authentication**      | ASP.NET Core Cookie Authentication           |
| **Password Hashing**    | BCrypt.Net-Next                              |
| **Object Mapping**      | AutoMapper                                   |
| **Validation**          | FluentValidation                             |
| **Logging**             | Serilog + File Sink                          |
| **Rich Text Editor**    | Quill.js                                     |
| **Frontend**            | Razor Views, HTML5, CSS3, Vanilla JavaScript |
| **Testing**             | xUnit, Moq, FluentAssertions                 |
| **Integration Testing** | SQLite In-Memory                             |
| **Version Control**     | Git / GitHub                                 |

---

## 🏗 Architecture

Roshdino follows a **Layered Architecture** and applies **Separation of Concerns** to keep the codebase maintainable and testable.

```text
                    ┌──────────────────────────┐
                    │   DigitalMarketing.Web   │
                    │          /               │
                    │   DigitalMarketing.Admin │
                    │      Presentation        │
                    └────────────┬─────────────┘
                                 │
                                 ▼
                    ┌──────────────────────────┐
                    │ DigitalMarketing.Services│
                    │   DTOs & Business Logic  │
                    └────────────┬─────────────┘
                                 │
                                 ▼
                    ┌──────────────────────────┐
                    │   DigitalMarketing.Data │
                    │   EF Core & Repositories│
                    └────────────┬─────────────┘
                                 │
                                 ▼
                    ┌──────────────────────────┐
                    │   DigitalMarketing.Core│
                    │   Entities & Interfaces │
                    └──────────────────────────┘
```

### Core

Contains domain entities and interfaces while remaining independent of infrastructure-specific implementation details.

### Data

Responsible for `DbContext`, Entity Framework Core configuration, migrations, repositories, and data access.

### Services

Contains DTOs, business logic, validation, and object mapping. This layer is shared between the Web and Admin applications.

### Web / Admin

Two independent ASP.NET Core MVC applications that use the shared application layers and connect to the same SQL Server database.

---

## 📁 Project Structure

```text
Roshdino/
│
├── src/
│   ├── DigitalMarketing.Core/
│   │   ├── Entities/
│   │   └── Interfaces/
│   │
│   ├── DigitalMarketing.Data/
│   │   ├── DbContext/
│   │   ├── Migrations/
│   │   └── Repositories/
│   │
│   ├── DigitalMarketing.Services/
│   │   ├── DTOs/
│   │   ├── Validators/
│   │   └── Business Logic/
│   │
│   ├── DigitalMarketing.Web/
│   │   └── Public Website
│   │
│   └── DigitalMarketing.Admin/
│       └── Admin Panel
│
└── tests/
    ├── DigitalMarketing.Services.Tests/
    └── DigitalMarketing.Data.Tests/
```

---

## ⚙️ Prerequisites

Make sure the following are installed:

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [SQL Server](https://www.microsoft.com/sql-server)
* [Visual Studio 2022](https://visualstudio.microsoft.com/) or another compatible IDE

> SQL Server Express is sufficient for local development.

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/mohamadvardan9/roshdino.git
cd roshdino
```

### 2. Configure the Connection String

Roshdino uses **ASP.NET Core User Secrets** to keep sensitive configuration outside the source code during development.

For the Admin project:

```bash
cd src/DigitalMarketing.Admin

dotnet user-secrets init

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING"
```

Repeat the same configuration for the Web project.

> Never commit real connection strings, passwords, API keys, or other secrets to the repository.

### 3. Configure File Uploads

Configure the upload directory in the `appsettings.json` files of both applications:

```json
{
  "Uploads": {
    "RootPath": "E:\\RoshdinoUploads",
    "RequestPath": "/uploads"
  }
}
```

Update RootPath to a valid directory on your local machine before running the application.

### 4. Apply Database Migrations

Using Visual Studio Package Manager Console:

```powershell
Add-Migration InitialCreate -StartupProject DigitalMarketing.Admin -Project DigitalMarketing.Data

Update-Database -StartupProject DigitalMarketing.Admin -Project DigitalMarketing.Data
```

### 5. Run the Applications

The `DigitalMarketing.Web` and `DigitalMarketing.Admin` applications can be configured as **Multiple Startup Projects** in Visual Studio and run simultaneously.

---

## 🧪 Running Tests

Run the Service tests:

```bash
dotnet test tests/DigitalMarketing.Services.Tests
```

Run the Data tests:

```bash
dotnet test tests/DigitalMarketing.Data.Tests
```

The test suite includes:

* Unit tests for service-layer business logic
* Repository integration tests
* Mocking with Moq
* FluentAssertions for readable assertions
* SQLite In-Memory for isolated database testing
* Testing database behaviors such as Soft Delete and Unique Constraints

---

## 🔒 Security

Roshdino includes several security-focused practices:

* Admin passwords are hashed using **BCrypt** and are never stored as plain text.
* The Admin application is protected by default using authorization filters.
* Controllers and actions require authorization unless explicitly marked with `[AllowAnonymous]`.
* Sensitive connection strings are not stored in `appsettings.json`.
* **User Secrets** are used for sensitive local development configuration.
* Production environments can use environment variables or an appropriate secret-management solution.
* The public contact form uses a **Honeypot** technique to reduce spam submissions.

---

## 🗺 Roadmap

* [x] Layered architecture
* [x] Database design and implementation
* [x] Admin authentication
* [x] Product management
* [x] Article management
* [x] Category management
* [x] Contact message management
* [x] Public website
* [x] SEO optimization
* [x] Automated testing
* [ ] Production deployment
* [ ] Email integration for contact messages

---

## 📄 License

This project is privately owned.

All rights reserved.

---

<div align="center">

**Built with ❤️ for Roshdino**

</div>
