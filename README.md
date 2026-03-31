# 📚 Library Management System — Setup Guide

## Tech Stack
- **Frontend**: C# Windows Forms (.NET Framework 4.7+)
- **Backend**: SQL Server (LocalDB / Express / Full)

---

## 📁 Project File Structure

```
LibraryManagementSystem/
│
├── Program.cs                  ← Application entry point
├── DatabaseHelper.cs           ← All DB connection & query helpers
├── SessionHelper.cs            ← Stores logged-in user info
│
├── LoginForm.cs                ← Login logic
├── LoginForm.Designer.cs       ← Login UI layout
│
├── RegisterForm.cs             ← Student self-registration
├── RegisterForm.Designer.cs
│
├── MainDashboard.cs            ← Main window with sidebar navigation
├── MainDashboard.Designer.cs
│
├── BooksControl.cs             ← View/Add/Edit/Delete books (UserControl)
├── StudentsControl.cs          ← View/Edit/Delete students (Admin only)
├── IssueBookControl.cs         ← Issue and Return books (Admin only)
├── MyBooksControl.cs           ← Student: view their issued books & browse
│
└── Database_Setup.sql          ← Run this first in SQL Server!
```

---

## ⚙️ Setup Steps

### Step 1 — Create the Database
1. Open **SQL Server Management Studio (SSMS)**
2. Open `Database_Setup.sql`
3. Run the entire script → this creates `LibraryDB`, all tables, stored procedures, and sample data

### Step 2 — Configure Connection String
Open `DatabaseHelper.cs` and update this line:
```csharp
private static string connectionString =
    "Server=YOUR_SERVER_NAME;Database=LibraryDB;Integrated Security=True;";
```
Replace `YOUR_SERVER_NAME` with your SQL Server instance name (e.g., `localhost`, `.\SQLEXPRESS`, `(localdb)\MSSQLLocalDB`).

### Step 3 — Create the Visual Studio Project
1. Open **Visual Studio**
2. Create a new **Windows Forms App (.NET Framework)** project
3. Name it: `LibraryManagementSystem`
4. Add all `.cs` files to the project (drag into Solution Explorer or use Add → Existing Item)
5. Install `System.Data.SqlClient` via NuGet if needed

### Step 4 — Run
Press **F5** to build and run.

---

## 🔐 Default Login

| Role  | Username | Password  |
|-------|----------|-----------|
| Admin | admin    | admin123  |

Students register via the **"New Student? Register Here"** button.

---

## ✅ Features

| Feature                  | Admin | Student |
|--------------------------|-------|---------|
| Login / Logout           | ✅    | ✅      |
| Register Account         | —     | ✅      |
| Manage Books (CRUD)      | ✅    | View only|
| Manage Students          | ✅    | —       |
| Issue Books to Students  | ✅    | —       |
| Return Books             | ✅    | —       |
| Browse Available Books   | —     | ✅      |
| View My Issued Books     | —     | ✅      |
| View My Borrowing History| —     | ✅      |
| Dashboard Stats          | ✅    | ✅      |
"# Library-Management-System" 
