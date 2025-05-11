## Note! The main branch is the basic project without
ONION, SOLID, CQRS (MediatR) Architecture, Testing, RxJS, and Material Module. 
### The complete project remains in the Development Branch for the sake of separation
---

# 📚 Angular + ASP.NET Core Book Tracker

This is a fullstack web application that allows users to manage their personal book library. Users can register, log in, and perform CRUD operations on books. The frontend is built with **Angular + Angular Material**, and the backend uses **ASP.NET Core Web API** with **Entity Framework Core** and **PostgreSQL**.

---

## 🚀 Tech Stack

### Frontend

* Angular 17
* Angular Material
* RxJS
* Firebase Hosting (optional)

### Backend

* ASP.NET Core 8
* Entity Framework Core
* PostgreSQL
* xUnit & Moq (unit testing)
* WebApplicationFactory (integration testing)

---

## 🔐 Features

* User authentication (JWT-based)
* CRUD operations on books
* Mark books as read/unread
* Responsive UI with Material Design
* Form validation (frontend + backend)
* CORS restricted to trusted domains

---

## 🛠️ Setup Instructions

### Backend (.NET API)

1. Navigate to the backend folder.
2. Update `appsettings.json` with your PostgreSQL connection string.
3. Run the migrations:

   ```bash
   dotnet ef database update
   ```
4. Launch the server:

   ```bash
   dotnet run
   ```

### Frontend (Angular)

1. Navigate to the Angular folder.
2. Install dependencies:

   ```bash
   npm install
   ```
3. Update `environment.ts` with your API base URL.
4. Start the dev server:

   ```bash
   ng serve
   ```

---

## 🌐 Deployment

* Frontend can be deployed on [Firebase Hosting](https://angularbook-8d451.web.app/books).
* Backend can be deployed on GCP, Azure, or DigitalOcean.
* PostgreSQL DB can be hosted using [Supabase](https://supabase.io/) or any managed DB service.

---

## 🧪 Testing

* Run unit tests:

  ```bash
  dotnet test
  ```
* Run integration tests with in-memory DB or WebApplicationFactory

