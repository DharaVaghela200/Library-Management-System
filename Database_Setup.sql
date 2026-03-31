-- ============================================
-- Library Management System - SQL Server Script
-- ============================================

CREATE DATABASE LibraryDB;
GO

USE LibraryDB;
GO

-- ========================
-- Table: Users
-- ========================
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Role NVARCHAR(10) NOT NULL CHECK (Role IN ('Admin', 'Student')),
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- ========================
-- Table: Students
-- ========================
CREATE TABLE Students (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE,
    Phone NVARCHAR(15),
    Address NVARCHAR(200),
    EnrollmentDate DATE DEFAULT GETDATE(),
    UserID INT FOREIGN KEY REFERENCES Users(UserID)
);
GO

-- ========================
-- Table: Books
-- ========================
CREATE TABLE Books (
    BookID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(100) NOT NULL,
    ISBN NVARCHAR(20) UNIQUE,
    Category NVARCHAR(50),
    Publisher NVARCHAR(100),
    TotalCopies INT NOT NULL DEFAULT 1,
    AvailableCopies INT NOT NULL DEFAULT 1,
    AddedDate DATE DEFAULT GETDATE()
);
GO

-- ========================
-- Table: IssuedBooks
-- ========================
CREATE TABLE IssuedBooks (
    IssueID INT IDENTITY(1,1) PRIMARY KEY,
    BookID INT NOT NULL FOREIGN KEY REFERENCES Books(BookID),
    StudentID INT NOT NULL FOREIGN KEY REFERENCES Students(StudentID),
    IssueDate DATE DEFAULT GETDATE(),
    DueDate DATE NOT NULL,
    ReturnDate DATE NULL,
    Status NVARCHAR(20) DEFAULT 'Issued' CHECK (Status IN ('Issued', 'Returned', 'Overdue'))
);
GO

-- ========================
-- Default Admin User
-- Password: admin123
-- ========================
INSERT INTO Users (Username, Password, Role) VALUES ('admin', 'admin123', 'Admin');
GO

-- ========================
-- Sample Books
-- ========================
INSERT INTO Books (Title, Author, ISBN, Category, Publisher, TotalCopies, AvailableCopies) VALUES
('C# Programming', 'Andrew Troelsen', '978-1430242338', 'Technology', 'Apress', 5, 5),
('Database Systems', 'Ramez Elmasri', '978-0133970777', 'Technology', 'Pearson', 3, 3),
('Clean Code', 'Robert C. Martin', '978-0132350884', 'Technology', 'Prentice Hall', 4, 4),
('The Great Gatsby', 'F. Scott Fitzgerald', '978-0743273565', 'Fiction', 'Scribner', 2, 2),
('Introduction to Algorithms', 'Thomas H. Cormen', '978-0262033848', 'Technology', 'MIT Press', 3, 3);
GO

-- ========================
-- Stored Procedure: Issue Book
-- ========================
CREATE PROCEDURE sp_IssueBook
    @BookID INT,
    @StudentID INT,
    @DueDate DATE
AS
BEGIN
    BEGIN TRANSACTION;
    INSERT INTO IssuedBooks (BookID, StudentID, DueDate, Status)
    VALUES (@BookID, @StudentID, @DueDate, 'Issued');

    UPDATE Books SET AvailableCopies = AvailableCopies - 1
    WHERE BookID = @BookID AND AvailableCopies > 0;
    COMMIT;
END;
GO

-- ========================
-- Stored Procedure: Return Book
-- ========================
CREATE PROCEDURE sp_ReturnBook
    @IssueID INT
AS
BEGIN
    BEGIN TRANSACTION;
    DECLARE @BookID INT;
    SELECT @BookID = BookID FROM IssuedBooks WHERE IssueID = @IssueID;

    UPDATE IssuedBooks SET ReturnDate = GETDATE(), Status = 'Returned'
    WHERE IssueID = @IssueID;

    UPDATE Books SET AvailableCopies = AvailableCopies + 1
    WHERE BookID = @BookID;
    COMMIT;
END;
GO
