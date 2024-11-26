# eGames

**eGames** is a game distribution platform inspired by Steam, allowing users to browse, purchase, and manage various types of games, including full versions, DLCs, and subscriptions. The application also provides features for managing orders, a user game library, user authorization, and role management.

## Features

- **Game Browsing and Filtering**: Users can browse available games, filter by different criteria, and search by keywords.
- **Product Types**: The platform supports full games, DLCs, and subscription-based games.
- **Cart and Orders**: Users can add games to a cart, place orders, and complete payments.
- **Game Library**: Users have access to their game library, where they can download purchased games and DLCs.
- **User and Role Management**: The system includes role-based authorization with roles such as user, admin, and super-admin.

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
- [https://egames-api-app-hyg9hzgbhka3dcct.polandcentral-01.azurewebsites.net/index.html](https://egames-api-app-hyg9hzgbhka3dcct.polandcentral-01.azurewebsites.net/index.html)
