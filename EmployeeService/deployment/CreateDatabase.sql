IF DB_ID(N'Test') IS NULL
BEGIN
    CREATE DATABASE [Test];
END
GO

USE [Test];
GO

IF OBJECT_ID(N'[dbo].[Employee]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Employee]
    (
        [ID] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Employee] PRIMARY KEY,
        [Name] NVARCHAR(100) NOT NULL,
        [ManagerID] INT NULL,
        [Enable] BIT NOT NULL CONSTRAINT DF_Employee_Enable DEFAULT (1),
        CONSTRAINT [FK_Employee_Manager]
            FOREIGN KEY ([ManagerID]) REFERENCES [dbo].[Employee]([ID])
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Employee])
BEGIN
    SET IDENTITY_INSERT [dbo].[Employee] ON;

    INSERT INTO [dbo].[Employee] ([ID], [Name], [ManagerID], [Enable])
    VALUES
        (1, N'Andrii', NULL, 1),
        (2, N'Oleksii', 1, 1),
        (3, N'Roman', 2, 1),
        (4, N'Serhii', 2, 1),
        (5, N'Dmytro', 3, 0),
        (6, N'Vladyslav', 3, 1);

    SET IDENTITY_INSERT [dbo].[Employee] OFF;
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Employee] WHERE [ID] = 7)
BEGIN
    SET IDENTITY_INSERT [dbo].[Employee] ON;

    INSERT INTO [dbo].[Employee] ([ID], [Name], [ManagerID], [Enable])
    VALUES
        (7, N'Kateryna', NULL, 1),
        (8, N'Bohdan', 7, 1),
        (9, N'Olena', 7, 1),
        (10, N'Mykola', 8, 1);

    SET IDENTITY_INSERT [dbo].[Employee] OFF;
END
GO
