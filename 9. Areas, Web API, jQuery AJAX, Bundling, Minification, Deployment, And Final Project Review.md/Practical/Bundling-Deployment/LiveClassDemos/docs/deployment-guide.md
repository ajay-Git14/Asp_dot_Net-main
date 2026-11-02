# Deployment Guide

```bash
dotnet build -c Release
dotnet publish -c Release -o ./publish
```

Configure the production connection string securely, set the environment to Production, deploy the publish folder, and test HTTPS, static files, database access, logs, and error handling.
