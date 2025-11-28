# AppsWave

# AppsWave E-Commerce API  

---

### Features

- User Registration & Login with JWT
- Role-based Authorization (`ADMIN` & `VISITOR`)
- Full Product CRUD (with pagination & soft delete)
- Purchase Cart → Generate Invoice
- View personal invoice history
- Global exception handling + structured logging
- Clean architecture with Repository + UnitOfWork pattern
- Database auto-migration + seed data

---

### Default Users

| Role    | Email                     | Password     |
|--------|---------------------------|--------------|
| Admin  | `admin@appswave.com`      | `Admin@123`  |
| User   | `visitor@appswave.com`    | `Visitor@123`|

---

### Tech Stack

- .NET 8.0
- Entity Framework Core (Code-First)
- SQL Server
- JWT Authentication
- OpenAPI
- Built-in ILogger + Global Exception Handler
