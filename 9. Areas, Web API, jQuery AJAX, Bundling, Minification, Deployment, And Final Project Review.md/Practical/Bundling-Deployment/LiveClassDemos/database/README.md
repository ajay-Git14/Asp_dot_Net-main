# Database Setup

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet ef migrations script -o database/schema.sql
```
