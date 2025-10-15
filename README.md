# DigitalLibrary

DigitalLibrary is a full-stack application for keeping track of books. Users can record the books they have read, track what they are currently reading, and maintain a personal library. The application is built with **ASP.NET Core**, **Angular**, and **Docker**, and includes a RESTful API, an SQLite database, Swagger documentation, asynchronous programming, and unit and integration testing.

---

## Features

- CRUD operations for library resources  
- RESTful API documented with **Swagger**  
- **SQLite** database with EF Core migrations  
- Asynchronous operations  
- Testing with **xUnit** and **Moq**  
- Backend containerized with **Docker**  
- Angular frontend (TypeScript)  

---

## Tech Stack

**Backend:** C#, ASP.NET Core Web API, Entity framework Core, SQLite, Swagger, Docker  
**Frontend:** Angular, TypeScript  
**Testing:** xUnit, Moq 

---

## Getting Started

### Docker

cd DigitalLibrary/DigitalLibrary.API
docker build -t digital-library-api .
docker run -p 5258:5258 digital-library-api

### Swagger UI

The API is documented using **Swagger** and can be accessed at: 
http://localhost:5258/swagger/index.html

### Frontend

### Testing

---

## Notes
- Database comes pre-seeded with demo data.
- Future additions will include book recommendations, yearly and monthly book reports, challenges and other user focused features.

