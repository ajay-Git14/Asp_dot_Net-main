# DockerNet10Demo

Working ASP.NET Core MVC project targeting .NET 10.

## Run Without Docker

```bash
dotnet restore
dotnet build
dotnet run
```

Open:

```text
http://localhost:5085
```

## Build Docker Image

```bash
docker build -t docker-net10-demo .
```

## Run Docker Container

```bash
docker run -d -p 8080:8080 --name docker-net10-container docker-net10-demo
```

Open:

```text
http://localhost:8080
```

Health endpoint:

```text
http://localhost:8080/health
```

## Logs

```bash
docker logs -f docker-net10-container
```

## Stop And Remove

```bash
docker stop docker-net10-container
docker rm docker-net10-container
```

## Docker Compose

```bash
docker compose up --build
```

Stop:

```bash
docker compose down
```
