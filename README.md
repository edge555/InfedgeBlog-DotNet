# InfedgeBlog
<br/>

This is a basic CRUD (Create, Read, Update, Delete) application for managing stories, users built using .NET 6.
<br/>

## Features
Create new story with a title, content.

Read and display existing stories.

Update existing story with revised content.

Delete existing story.

<br/>

Create new user.

Update existing users.

Delete existing users.

<br/>

## Technologies Used
.NET 6
ASP.NET Core MVC
Entity Framework Core
SQL Server

<br/>

## Setup Instructions

To run the application locally, please follow these steps:

Clone the repository.

Ensure you have .NET 6 SDK installed on your machine.

Update the connection string in the appsettings.json file to connect to your desired database.

Open a terminal or command prompt in the project directory.

Run the following commands:
```
dotnet restore
dotnet build
dotnet run
```

Open a web browser and navigate to http://localhost:7169 to access the application.

<br/>

## API Endpoints:

POST /api/story - Create a new story.

GET /api/story - Retrieve all stories.

GET /api/story/{id} - Retrieve a specific story by ID.

PUT /api/story/{id} - Update a specific story by ID.

DELETE /api/story/{id} - Delete a specific story by ID.

<br/>

POST /api/auth/signup - Create a new user.

POST /api/auth/login - Login a existing user.

<br/>

GET /api/user - Retrieve all stories.

GET /api/user/{id} - Retrieve a specific user by ID.

PUT /api/user/{id} - Update a specific user by ID.

DELETE /api/user/{id} - Delete a specific user by ID.

<br/>

## Folder Structure

Controllers: Contains the controller classes responsible for handling HTTP requests and responses.

Models: Defines the data models used in the application.

Views: Contains the Razor views for displaying the UI.

Database: Contains the data access layer, including the DbContext and migrations.

Services: Provides the business logic and services for the application.

<br/>

## License
This project is licensed under the MIT License. Feel free to use and modify the code as per your requirements.

<br/>

## Contact
For any inquiries or further information, please contact [2010.ahmed.shoaib@gmail.com].
