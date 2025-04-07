using Dapper;
using Microsoft.Extensions.Configuration;

namespace Taxi.Repository.Sql;

public static class Initialization
{
    public static async Task InitializeDatabase(IConfiguration configuration)
    {
        using var connection = SqlConnectionBuilder.GetConnection(configuration);

        // Create Drivers table
        await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Drivers')
                BEGIN
                    CREATE TABLE Drivers (
                        Id NVARCHAR(50) PRIMARY KEY,
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
                END");

        // Create DriverVehicles table
        await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DriverVehicles')
                BEGIN
                    CREATE TABLE DriverVehicles (
                        Id NVARCHAR(50) PRIMARY KEY,
                        DriverId NVARCHAR(50) NOT NULL,
                        VehicleType NVARCHAR(50) NOT NULL,
                        Model NVARCHAR(100) NOT NULL,
                        RegistrationNumber NVARCHAR(20) NOT NULL,
                        Color NVARCHAR(50) NOT NULL,
                        CONSTRAINT FK_DriverVehicles_Drivers FOREIGN KEY (DriverId) REFERENCES Drivers(Id)
                    )
                END");

        // Create Passengers table
        await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Passengers')
                BEGIN
                    CREATE TABLE Passengers (
                        Id NVARCHAR(50) PRIMARY KEY,
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
                END");
    }
}
