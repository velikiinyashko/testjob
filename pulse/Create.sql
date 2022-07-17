-- Create a new table called 'Product' in schema 'dbo'
-- Drop the table if it already exists
IF OBJECT_ID('dbo.Product', 'U') IS NOT NULL
DROP TABLE dbo.Product

-- Create the table in the specified schema
CREATE TABLE dbo.Product
(
    ProductId INT NOT NULL PRIMARY KEY IDENTITY, -- primary key column
    [Name] [NVARCHAR](256) NOT NULL,
    [DateCreate] DATETIME2 DEFAULT GETDATE(),

);


-- Create a new table called 'Stock' in schema 'dbo'
-- Drop the table if it already exists
IF OBJECT_ID('dbo.Stock', 'U') IS NOT NULL
DROP TABLE dbo.Stock

-- Create the table in the specified schema
CREATE TABLE dbo.Stock
(
    [StockId] INT NOT NULL PRIMARY KEY IDENTITY, -- primary key column
    [RetailId] INT NOT NULL,
    [Name] [NVARCHAR](256) NOT NULL,
    [DateCreate] DATETIME2 DEFAULT GETDATE(),

);


-- Create a new table called 'Retail' in schema 'dbo'
-- Drop the table if it already exists
IF OBJECT_ID('dbo.Retail', 'U') IS NOT NULL
DROP TABLE dbo.Retail

-- Create the table in the specified schema
CREATE TABLE dbo.Retail
(
    [RetailId]  INT NOT NULL PRIMARY KEY IDENTITY, -- primary key column
    [Name] [NVARCHAR](256) NOT NULL,
    [City] [NVARCHAR](50) NOT NULL,
    [Address] [NVARCHAR](256) NOT NULL,
    [Phone] [NVARCHAR](20),
    [DateCreate] DATETIME2 DEFAULT GETDATE(),

);


-- Create a new table called 'Party' in schema 'dbo'
-- Drop the table if it already exists
IF OBJECT_ID('dbo.Party', 'U') IS NOT NULL
DROP TABLE dbo.Party

-- Create the table in the specified schema
CREATE TABLE dbo.Party
(
    PartyId INT NOT NULL PRIMARY KEY IDENTITY, -- primary key column
    [ProductId] INT NOT NULL,
    [StockId] INT NOT NULL,
    [Count] INT NOT NULL DEFAULT 0,
    [DateCreate] DATETIME2 DEFAULT GETDATE(), 
);


