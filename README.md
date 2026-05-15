# Library Management System

This project is a web-based Library Management System developed for a Software Engineering course at Sakarya University. It is built using ASP.NET Core MVC and Entity Framework Core.

## Project Team
* Ayşe Dönmez - 251202350)
* Ümit Ak - 241202351)

## Features
* **Authentication:** User registration and login managed by ASP.NET Core Identity.
* **Book Management:** CRUD operations (Add, Edit, Delete, View) for the library catalog.
* **Borrowing System:** Users can borrow available books and track their reading history.
* **Database:** Relational data storage using SQLite and Code-First approach.

## Technologies Used
* C# & ASP.NET Core MVC (.NET 9.0)
* Entity Framework Core
* HTML5, CSS3, Bootstrap 5

## How to Run
1. Clone this repository to your local machine.
2. Open your terminal and navigate to the project folder.
3. Run `dotnet restore` to install the required dependencies.
4. Run `dotnet ef database update` to create the database (if needed).
5. Run `dotnet run` to start the application.
6. Open your web browser and go to the provided localhost address.
