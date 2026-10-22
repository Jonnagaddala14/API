# RandomApi

A small .NET 10 minimal API that returns random JSON. Built as a sandbox for testing the AI Agent Workflow kit.

## Run

```
dotnet run --project src/RandomApi
dotnet test
```

## Endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/health` | Health check |
| GET | `/api/users?count=5` | Random users (count 1–100, default 5) |
| GET | `/api/users/{id}` | A random user with the given id |
| GET | `/api/quote` | A random quote |
| GET | `/api/weather/{city}` | Random weather for a city |
| POST | `/api/echo` | Echoes `{ "message": "..." }` back with metadata |

OpenAPI document (Development only): `/openapi/v1.json`
