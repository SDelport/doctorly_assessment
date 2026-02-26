
# 📦 doctorly Assessment Project



## 🚀 Overview

-   ASP.NET Core Web API
    
-   Runs on **IIS**
    
-   Uses **Entity Framework Core**
    
-   Database: **SQLite**
    
-   Unit tests using **xUnit**
    
-   API documentation via **Swagger / OpenAPI**
       

----------

## 📚 API Documentation

Swagger UI is available at:

    /swagger

When the application is running, navigate to:

https://localhost:7118/swagger

The **OpenAPI document (JSON)** link is available directly from the Swagger page.

----------

## 🗄 Database

-   Provider: **SQLite**
    
-   ORM: **Entity Framework Core**
    
-   Storage Location:
    
    %APPDATA%\doctorly_assessment.db


The database file is automatically created when migrations are applied.


----------

## 🧪 Running Tests

The project includes **xUnit** tests.

To execute the test suite:

dotnet test

This will build the solution and run all unit/integration tests.

----------

## 🛠 Running the Application

### 1️⃣ Restore Dependencies

    dotnet restore

### 2️⃣ Apply Migrations

Apply migrations with:

    dotnet ef database update

### 3️⃣ Run the Application

    dotnet run


----------

## 🧱 Technology Stack

-   **ASP.NET Core**
    
-   **Entity Framework Core**
    
-   **SQLite**
    
-   **xUnit**
    
-   **Swagger / OpenAPI**
    
    

----------

## 📝 Notes

    
-   Follows RESTful API principles.
    
-   Includes automated tests.
    
-   Uses SQLite for lightweight local persistence.
    
-   Implements basic optimistic locking for safe concurrent updates.
    