# MediStock 💊
### Medicine Inventory Management System

A full-featured pharmacy inventory management system built with ASP.NET Core 8 MVC. MediStock helps pharmacies manage their medicine inventory, track sales, manage suppliers and customers, and maintain a complete audit trail of all stock movements.

---

## Features

- **Dashboard** — Real-time overview of sales, revenue, low stock alerts, expiring products, and quick actions
- **Products** — Full medicine inventory with SKU tracking, expiry dates, dosage forms, reorder levels, and automatic status management
- **Categories** — Organize products into categories
- **Suppliers** — Manage medicine suppliers with status tracking (Active, Inactive, Blacklisted)
- **Customers** — Customer management with type classification (Regular, VIP, Wholesale, Insurance)
- **Sales** — Point-of-sale with dynamic line items, automatic stock deduction, VAT calculation, and change calculation
- **Purchase Orders** — Restock from suppliers with receive workflow that automatically updates stock
- **Stock Adjustments** — Manual stock adjustments with full audit trail
- **Reports** — Sales reports with date range filtering and stock reports with expiry/low stock highlights
- **User Management** — Manage staff accounts with role-based access (Admin, Pharmacist, Cashier)
- **System Logs** — Audit trail of system activity
- **Authentication** — Secure login with ASP.NET Core Identity

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 8 MVC |
| Language | C# 12 |
| ORM | Entity Framework Core 8 |
| Database | SQL Server (LocalDB) |
| Authentication | ASP.NET Core Identity |
| Frontend | Bootstrap 5 + Bootstrap Icons 1.11.3 |
| CSS | Custom design system (medistock.css) |

---

## Architecture

```
Controller → Service → Repository → DbContext
```

- **Controllers** — Thin, only call services and pass data to views
- **Services** — All business logic, return `(bool Success, string Message)` tuples
- **Repositories** — Data access only, generic + specific interfaces
- **Unit of Work** — All repositories share one DbContext per request
- **ViewModels** — Entities are never passed directly to views

### Project Structure

```
Simplified.Medistock/
├── Controllers/
├── Services/
│   ├── Interfaces/
│   └── Implementations/
├── Repositories/
│   ├── Interfaces/
│   └── Implementations/
├── Models/
│   ├── Entities/
│   ├── ViewModels/
│   └── Enums/
├── Data/
│   └── AppDbContext.cs
├── Views/
│   ├── Shared/
│   ├── Home/          (Dashboard)
│   ├── Categories/
│   ├── Products/
│   ├── Suppliers/
│   ├── Customers/
│   ├── Sales/
│   ├── StockAdjustments/
│   ├── PurchaseOrders/
│   ├── Reports/
│   ├── Users/
│   └── SystemLogs/
└── wwwroot/
    └── css/
        └── medistock.css
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (included with Visual Studio)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/medistock.git
cd medistock
```

2. **Configure the database connection**

Open `appsettings.json` and update the connection string if needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MediStockDb;Trusted_Connection=True;"
  }
}
```

3. **Apply migrations**

Open the Package Manager Console in Visual Studio and run:
```powershell
Update-Database
```

Or using the .NET CLI:
```bash
dotnet ef database update
```

4. **Run the application**
```bash
dotnet run
```

5. **Register your first account**

Navigate to `/Identity/Account/Register` to create your first user account, then log in.

---

## Key Business Rules

- **Soft Delete** — Records are never physically deleted. A global `IsDeleted` query filter hides deleted records automatically.
- **Stock Adjustments** — Stock can only be changed through StockAdjustments or Sales — never by directly editing a product.
- **Sales are permanent** — Once a sale is created it cannot be edited or deleted. Stock is reduced immediately.
- **Purchase Orders** — Stock is only added when a PO is marked as Received, not when it is created.
- **Atomic Transactions** — All operations (e.g. creating a sale, reducing stock, creating adjustment records) happen in a single `SaveChangesAsync` call — all or nothing.
- **Unique constraints** — Category names, Product SKUs, Sale numbers, and PO numbers are all unique with soft-delete awareness.

---

## Database Schema

### Core Entities

| Entity | Description |
|--------|-------------|
| `Category` | Product categories |
| `Product` | Medicine inventory items |
| `Supplier` | Medicine suppliers |
| `Customer` | Pharmacy customers |
| `Sale` | Sales transactions |
| `SaleItem` | Individual line items within a sale |
| `PurchaseOrder` | Supplier restock orders |
| `PurchaseOrderItem` | Individual line items within a PO |
| `StockAdjustment` | Manual stock change records |
| `SystemLog` | System audit trail |

All entities except `SystemLog` inherit from `BaseEntity` which provides:
- `Id` — Primary key
- `CreatedAt` / `UpdatedAt` — Timestamps
- `CreatedBy` / `UpdatedBy` — User tracking
- `IsDeleted` — Soft delete flag

---

## License

This project is for educational purposes.

---

## Author

Built as a learning project to practice ASP.NET Core 8 MVC, Entity Framework Core, Repository Pattern, and Clean Architecture principles.
