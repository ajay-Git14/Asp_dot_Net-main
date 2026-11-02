# LiveClassDemos - Web API and jQuery AJAX

A complete beginner-friendly ASP.NET Core MVC practical using:

- .NET 8
- ASP.NET Core MVC
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- jQuery AJAX

## Why SQLite?

SQLite makes this project immediately runnable without installing or configuring SQL Server. The database file is created automatically as `complaints.db` when the app starts.

## Requirements

- .NET 8 SDK
- Visual Studio 2022, VS Code, or command prompt
- Internet access during the first `dotnet restore`

## Run from command line

```bash
dotnet restore
dotnet run
```

Open the URL printed in the terminal, normally:

```text
http://localhost:5082
```

## Main URLs

```text
/AjaxDemo
/api/complaints
/api/dashboard/summary
```

## Reset sample data

1. Stop the project.
2. Delete `complaints.db`.
3. Run `dotnet run` again.

The project recreates the database and inserts three sample complaints.

## API tests

### GET complaints

```http
GET /api/complaints
```

### GET filtered complaints

```http
GET /api/complaints?status=New
```

### POST complaint

```json
{
  "title": "Water leakage",
  "description": "Water is leaking near the parking area.",
  "category": "Water",
  "priority": "High"
}
```

### PUT complaint status

```json
{
  "newStatus": "InProgress",
  "remarks": "Work started."
}
```

Endpoint:

```http
PUT /api/complaints/1/status
```
