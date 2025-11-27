# DeveloperAgent Clean Architecture .NET Web API

This repository is a scaffold for a .NET Web API using Clean Architecture principles. It includes the following projects:

- src/DeveloperAgent.Domain - Domain entities and interfaces
- src/DeveloperAgent.Application - Use cases, DTOs, interfaces
- src/DeveloperAgent.Infrastructure - EF Core (InMemory) and repository implementations
- src/DeveloperAgent.Web - Web API project
- tests - unit tests

## Getting Started

Prerequisites: .NET SDK 8.0 or later

From PowerShell:

```powershell
# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run --project src\DeveloperAgent.Web
```

This sample uses an InMemory database for simplicity.

---

Contributions welcome. This scaffold is intentionally small to show the Clean Architecture layers.

---
These are the commands for executing the below agents
 
@codereviewagent scan entire codebase for security vulnerabilities
 
@developeragent create .NET Web API with clean architecture
 
@codefixagent fix security issues and rewrite this controller with best practices

@requirementsagent Break down this business requirement into epics, user stories, and acceptance criteria with gap analysis.

@unittestagent Generate unit tests

@dataschemaagent Generate database schemas and entity models for this user management system with audit trails.

@loggingagent Insert structured logging, telemetry, and monitoring hooks into this application with dashboard setup.

@architectureagent Design a microservices architecture for this e-commerce system with API contracts and data models.

---
