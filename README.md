# A House Rent Management System

A web-based CRUD application built with ASP.NET Core MVC for managing house rental listings. Users can register, log in, post properties, add comments and ratings; admins can approve or reject listings and manage areas, users, and reports.

---

## Table of Contents

1. [Features](#features)  
2. [Tech Stack](#tech-stack)  
3. [Prerequisites](#prerequisites)  
4. [Setup & Installation](#setup--installation)  
5. [Database Migrations & Seeding](#database-migrations--seeding)  
6. [Running the Application](#running-the-application)  
7. [Project Structure](#project-structure)  
8. [Contributing](#contributing)  
9. [License](#license)

---

## Features

- **User Registration & Login**  
  - Passwords are hashed with ASP.NET Core Identity’s `IPasswordHasher`.  
  - “Remember me” cookie for 7-day auto-login.  
- **User Roles**  
  - `User`: create/edit/delete own listings; add comments & ratings.  
  - `Admin`: approve/pending listings; manage areas, users, and view reports.  
- **Property Management**  
  - CRUD for properties with up to 5 images.  
  - Filter/search by region, price, area size.  
- **Comments & Ratings**  
  - Authenticated users can comment and rate properties (0–5).  
  - Comments & ratings sorted newest first.  
- **Admin Panel**  
  - Tabs for Approved, Pending posts, Areas, Users, Admins, Reports.  
- **Responsive, Themed UI**  
  - Dark “Nord” theme with Bootstrap.  

---

## Tech Stack

- **Backend**: ASP.NET Core 6 MVC  
- **ORM**: Entity Framework Core  
- **Database**: SQL Server (LocalDB or full SQL)  
- **Frontend**: Razor Views + Bootstrap 5 + jQuery  
- **Authentication**: ASP.NET Core Identity (only `IPasswordHasher<Person>`)  
- **Version Control**: Git / GitHub  

---

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)  
- [SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) or full SQL Server  
- Git  

---

## Setup & Installation

1. **Clone the repo**  
   ```bash
   git clone https://github.com/your-user/A_house_Rent_management_system.git
   cd A_house_Rent_management_system
   ```

2. **Configure your connection string**  
   In `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "ApartmentConnectionString": "Server=YOUR_SERVER;Database=FinalDB;Trusted_Connection=True;"
   }
   ```

3. **Install dependencies**  
   ```bash
   dotnet restore
   ```

---

## Database Migrations & Seeding

1. **Create/Update the database schema**  
   ```bash
   dotnet ef database update
   ```

2. **Seed initial data**  
   A static seed in code (e.g. in `Program.cs` or a custom initializer) hashes default passwords and inserts:
   ```csharp
   var users = new[] {
       new { First="John", Last="Doe", ... },
       /* ...all 25 users... */
       new { First="Tina", Last="Lewis", ... }
   };
   foreach (var u in users) {
       var person = new Person {
         FullName  = $"{u.First} {u.Last}",
         nationalID= u.NationalID,
         phoneNumber = u.Phone,
         email     = u.Email,
         PasswordHash = passwordHasher.HashPassword(null, u.Password),
         AccountType = "User"
       };
       context.Persons.Add(person);
   }
   await context.SaveChangesAsync();
   ```

3. **Seed sample properties**  
   Similarly insert sample listings:
   ```csharp
   var props = new[] {
     new { Price=250000m, Area=150.5f, ... },
     /* ... up to 15 sample properties ... */
   };
   foreach (var p in props) {
     context.Properties.Add(new Property {
       Price = p.Price,
       Area  = p.Area,
       /* … other fields … */
       CreatedAt = DateTime.UtcNow,
       HireStatus= 0
     });
   }
   await context.SaveChangesAsync();
   ```

---

## Running the Application

```bash
dotnet run
```

Then browse to `https://localhost:5001` (or the port shown in console).

---

## Project Structure

```
/
├── Controllers/
│   ├── RegistrationController.cs
│   ├── LoginController.cs
│   ├── UserAccountController.cs
│   ├── UserHomeController.cs
│   ├── AdminPanelController.cs
│   └── PropertyController.cs
├── Models/
│   ├── Person.cs
│   ├── Property.cs
│   ├── Comments.cs
│   ├── PropertyRating.cs
│   ├── Area.cs
│   └── SearchHistory.cs
├── Controllers/DTOs/
│   ├── PersonInput.cs
│   ├── LoginInput.cs
│   ├── ProfileInput.cs
│   ├── PropertyInput.cs
│   └── PropertyEditInput.cs
├── Views/
│   ├── Registration/
│   ├── Login/
│   ├── UserAccount/
│   ├── UserHome/
│   ├── AdminPanel/
│   └── Shared/_Layout.cshtml
├── DBContext/
│   └── AppDbContext.cs
├── Migrations/
├── wwwroot/
│   └── images/
├── appsettings.json
└── A_house_Rent_management_system.sln
```

---

## Contributing

1. Fork the repo  
2. Create your feature branch (`git checkout -b feature/xyz`)  
3. Commit your changes (`git commit -m "feat: add xyz"`)  
4. Push to branch (`git push origin feature/xyz`)  
5. Open a Pull Request

---

## License

This project is licensed under the MIT License.  
Feel free to use, modify, and distribute!