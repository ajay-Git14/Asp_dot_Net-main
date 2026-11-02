# LiveClassDemos - .NET 10 MVC Final Practical

This project demonstrates bundling, minification, static files, Development versus Production, deployment preparation, and documentation.

## Run

```bash
dotnet restore
dotnet build
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

Open `http://localhost:5082`.

## Publish

```bash
dotnet publish -c Release -o ./publish
```

The production bundle files are included. Regenerate them whenever source CSS or JavaScript changes.
