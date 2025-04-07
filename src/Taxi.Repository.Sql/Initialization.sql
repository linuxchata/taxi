CREATE TABLE Drivers (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    Country NVARCHAR(50) NOT NULL,
    State NVARCHAR(50) NOT NULL,
    Rating DECIMAL(3,2) NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    IsApproved BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME2 NOT NULL,
    UpdatedDate DATETIME2 NOT NULL,
    Version INT NOT NULL DEFAULT 1
)

CREATE TABLE DriverVehicles (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    DriverId UNIQUEIDENTIFIER NOT NULL,
    VehicleType NVARCHAR(50) NOT NULL,
    Model NVARCHAR(100) NOT NULL,
    RegistrationNumber NVARCHAR(20) NOT NULL,
    Color NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_DriverVehicles_Drivers FOREIGN KEY (DriverId) REFERENCES Drivers(Id)
)

CREATE TABLE Passengers (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    Country NVARCHAR(50) NOT NULL,
    State NVARCHAR(50) NOT NULL,
    Rating DECIMAL(3,2) NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME2 NOT NULL,
    UpdatedDate DATETIME2 NOT NULL,
    Version INT NOT NULL DEFAULT 1
)