### Features
- User registration & login with JWT
- Role-based authorization (`ADMIN` / `VISITOR`)
- Complete Product CRUD with soft delete & pagination
- Shopping cart → Invoice generation
- View personal invoice history
- Global exception handling with structured JSON responses
- Structured logging using built-in `ILogger`
- Clean Architecture + Repository + UnitOfWork pattern
- Auto database migration + seed data on startup

---

### Default Credentials

| Role   | Email                      | Password     |
|-------|----------------------------|--------------|
| Admin | `admin@appswave.com`       | `Admin@123`  |
| User  | `visitor@appswave.com`     | `Visitor@123`|

---

### Tech Stack
- .NET 8.0
- Entity Framework Core (Code First)
- SQL Server
- JWT Bearer Authentication
- OpenAPI
- Built-in Logging & Global Exception Middleware

---

### How to Run

```bash
git clone https://github.com/laiiith/AppsWave.git
cd AppsWave-ECommerce-API/src/AppsWave
dotnet restore
dotnet run
→ Database is created and seeded automatically

### API Endpoints Overview

| Method | Endpoint                        | Description                        | Required Role     |
|--------|----------------------------------|------------------------------------|-------------------|
| **POST**   | `/api/auth/register`             | Register a new user                | Public            |
| **POST**   | `/api/auth/login`                | Login → returns JWT token          | Public            |
| **GET**    | `/api/products`                  | List all products (paginated)      | Public            |
| **GET**    | `/api/products/{id}`             | Get product by ID                  | Public            |
| **POST**   | `/api/products`                  | Create new product                 | **ADMIN**         |
| **PUT**    | `/api/products/{id}`             | Update existing product            | **ADMIN**         |
| **DELETE** | `/api/products/{id}`             | Soft delete product                | **ADMIN**         |
| **POST**   | `/api/invoice/purchase`          | Purchase items → generate invoice  | **VISITOR / ADMIN** |
| **GET**    | `/api/invoice/my-invoices`       | Get user's invoice history         | **VISITOR / ADMIN** |

### Postman Collection (Ready to Use)

**File:** `Postman/AppsWave E-Commerce API.postman_collection.json`

**How to use:**

1. Open Postman → Click **Import** → Choose the file from the `Postman/` folder
2. Run the request **Login**
   → JWT token is **automatically saved** to the `{{token}}` variable
3. Now click any other request → everything works instantly!

**No manual token copying needed — fully automated**

All endpoints are pre-configured, grouped, and tested:
- Auth
- Products (Public)
- Products (ADMIN ONLY)
- Invoice flow

Just import → login → test everything.