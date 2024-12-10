# eGames API Server

## Technologies

Key technologies used in the project:

- **.NET 9** and **ASP.NET Core** for web application and API development.
- **Entity Framework Core** for database management and migrations.
- **ASP.NET Identity** for user authentication and role-based access control.
- **MediatR** for implementing the Mediator pattern.
- **FluentValidation** for data validation.
- **Swagger** (Swashbuckle) for API documentation.
- **NLog** for logging.

## Project Structure

The project is organized according to Domain-Driven Design (DDD) and Clean Architecture principles, divided into three main components:

- **Core**: Contains the domain logic, application use cases, and data transfer objects (DTOs).
- **External**: Handles infrastructure such as database management and API routing.
- **Web**: The main ASP.NET Core project, responsible for hosting the application and configuring API documentation.

## Deployment

The application is deployed on Azure:

- **Azure Web App**: Hosts the eGames application.
- **Azure SQL Database**: Stores application data.

## API Documentation

The API documentation is available via Swagger and can be accessed at:
