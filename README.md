# SMSF Audit Platform

A full-stack web application built with **ASP.NET Core**, **Blazor**, and **Entity Framework Core** to streamline Self-Managed Superannuation Fund (SMSF) auditing workflows.

Built as a portfolio project to demonstrate C#/.NET development skills relevant to RegTech and audit software.

---

## Features

- **Audit Session Management** - Create and manage audit sessions per SMSF fund
- **Interactive Checklists** - Structured audit checklists with progress tracking and notes
- **Dashboard** - Visual progress overview with Chart.js doughnut chart and per-fund progress bars
- **AI Assistant** - GPT-powered SMSF compliance assistant with streaming output
- **PDF Export** - Generate professional audit reports using QuestPDF
- **User Authentication** - Secure registration and login with ASP.NET Core Identity

---

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Blazor Server (.NET 10) |
| Backend | ASP.NET Core, C# |
| Database | SQLite + Entity Framework Core |
| Authentication | ASP.NET Core Identity |
| AI | OpenAI API (GPT-4o mini) |
| PDF | QuestPDF |
| UI | Bootstrap 5, Bootstrap Icons, Chart.js |

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- OpenAI API key ([get one here](https://platform.openai.com/api-keys))

### Setup

1. Clone the repository and enter the directory

2. Add your OpenAI API key to appsettings.Development.json under the OpenAI:ApiKey field

3. Run database migrations with: dotnet ef database update

4. Start the app with: dotnet run

5. Open http://localhost:5297 in your browser

---

## Project Structure

- Components/ - Blazor pages and layout
- Data/ - EF Core DbContext
- Models/ - Domain models
- Pages/ - Razor Pages for server-side login
- Services/ - ChecklistService, PdfService
- wwwroot/ - Static assets

---

## Roadmap

- Role-based access control (Admin / Auditor)
- Email notifications
- Azure deployment
- Azure OpenAI migration

---

## License

MIT