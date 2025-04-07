using Dapper;
using Microsoft.Extensions.Configuration;
using Taxi.Core.Infrastructure;
using Taxi.Domain;

namespace Taxi.Repository.Sql;

public sealed class DriverRepository : IDriverRepository
{
    private readonly IConfiguration _configuration;

    public DriverRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<Driver>> GetAll()
    {
        using var connection = SqlConnectionBuilder.GetConnection(_configuration);

        var drivers = await connection.QueryAsync<Driver>(@"
            SELECT 
                Id, 
                FirstName, 
                LastName, 
                Email, 
                PhoneNumber, 
                Country, 
                State, 
                Rating, 
                IsActive, 
                IsApproved, 
                CreatedDate, 
                UpdatedDate, 
                'Driver' AS Type, 
                Version
            FROM Drivers");

        // Get vehicles for each driver
        foreach (var driver in drivers)
        {
            var vehicles = await connection.QueryAsync<DriverVehicle>(@"
                SELECT 
                    Id, 
                    VehicleType, 
                    Model, 
                    RegistrationNumber, 
                    Color
                FROM DriverVehicles
                WHERE DriverId = @DriverId",
                new { DriverId = driver.Id });

            driver.Vehicles = vehicles;
        }

        return drivers;
    }

    public async Task<Driver> GetById(string id)
    {
        using var connection = SqlConnectionBuilder.GetConnection(_configuration);

        var driver = await connection.QuerySingleOrDefaultAsync<Driver>(@"
            SELECT 
                Id, 
                FirstName, 
                LastName, 
                Email, 
                PhoneNumber, 
                Country, 
                State, 
                Rating, 
                IsActive, 
                IsApproved, 
                CreatedDate, 
                UpdatedDate, 
                'Driver' AS Type, 
                Version
            FROM Drivers
            WHERE Id = @Id",
            new { Id = id });

        if (driver == null)
        {
            return null!;
        }

        // Get vehicles for the driver
        var vehicles = await connection.QueryAsync<DriverVehicle>(@"
            SELECT 
                Id, 
                VehicleType, 
                Model, 
                RegistrationNumber, 
                Color
            FROM DriverVehicles
            WHERE DriverId = @DriverId",
            new { DriverId = id });

        driver.Vehicles = vehicles;
        driver.Pk = $"Driver:{driver.Id.ToLower()}"; // Set partition key for compatibility

        return driver;
    }

    public async Task<string> Create(Driver driver)
    {
        // Generate new ID if not provided
        if (string.IsNullOrEmpty(driver.Id))
        {
            driver.Id = Guid.NewGuid().ToString().ToLower();
        }

        driver.CreatedDate = DateTime.UtcNow;
        driver.UpdatedDate = driver.CreatedDate;

        using var connection = SqlConnectionBuilder.GetConnection(_configuration);
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(@"
                INSERT INTO Drivers (
                    Id, 
                    FirstName, 
                    LastName, 
                    Email, 
                    PhoneNumber, 
                    Country, 
                    State, 
                    Rating, 
                    IsActive, 
                    IsApproved, 
                    CreatedDate, 
                    UpdatedDate, 
                    Version
                ) VALUES (
                    @Id, 
                    @FirstName, 
                    @LastName, 
                    @Email, 
                    @PhoneNumber, 
                    @Country, 
                    @State, 
                    @Rating, 
                    @IsActive, 
                    @IsApproved, 
                    @CreatedDate, 
                    @UpdatedDate, 
                    @Version
                )",
                driver,
                transaction);

            // Insert vehicles if any
            if (driver.Vehicles != null && driver.Vehicles.Any())
            {
                foreach (var vehicle in driver.Vehicles)
                {
                    if (string.IsNullOrEmpty(vehicle.Id))
                    {
                        vehicle.Id = Guid.NewGuid().ToString().ToLower();
                    }

                    await connection.ExecuteAsync(@"
                        INSERT INTO DriverVehicles (
                            Id, 
                            DriverId, 
                            VehicleType, 
                            Model, 
                            RegistrationNumber, 
                            Color
                        ) VALUES (
                            @Id, 
                            @DriverId, 
                            @VehicleType, 
                            @Model, 
                            @RegistrationNumber, 
                            @Color
                        )",
                        new
                        {
                            vehicle.Id,
                            DriverId = driver.Id,
                            vehicle.VehicleType,
                            vehicle.Model,
                            vehicle.RegistrationNumber,
                            vehicle.Color
                        },
                        transaction);
                }
            }

            transaction.Commit();
            return driver.Id;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<Driver> Update(string id, Driver driver)
    {
        driver.Id = id.ToLower();
        driver.UpdatedDate = DateTime.UtcNow;

        using var connection = SqlConnectionBuilder.GetConnection(_configuration);
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // Check if driver exists
            var exists = await connection.ExecuteScalarAsync<bool>(
                "SELECT COUNT(1) FROM Drivers WHERE Id = @Id",
                new { Id = id },
                transaction);

            if (!exists)
            {
                return null!;
            }

            // Update driver
            await connection.ExecuteAsync(@"
                UPDATE Drivers SET
                    FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email,
                    PhoneNumber = @PhoneNumber,
                    Country = @Country,
                    State = @State,
                    Rating = @Rating,
                    IsActive = @IsActive,
                    IsApproved = @IsApproved,
                    UpdatedDate = @UpdatedDate,
                    Version = @Version
                WHERE Id = @Id",
                driver,
                transaction);

            // Delete existing vehicles
            await connection.ExecuteAsync(
                "DELETE FROM DriverVehicles WHERE DriverId = @DriverId",
                new { DriverId = id },
                transaction);

            // Insert updated vehicles if any
            if (driver.Vehicles != null && driver.Vehicles.Any())
            {
                foreach (var vehicle in driver.Vehicles)
                {
                    if (string.IsNullOrEmpty(vehicle.Id))
                    {
                        vehicle.Id = Guid.NewGuid().ToString().ToLower();
                    }

                    await connection.ExecuteAsync(@"
                        INSERT INTO DriverVehicles (
                            Id, 
                            DriverId, 
                            VehicleType, 
                            Model, 
                            RegistrationNumber, 
                            Color
                        ) VALUES (
                            @Id, 
                            @DriverId, 
                            @VehicleType, 
                            @Model, 
                            @RegistrationNumber, 
                            @Color
                        )",
                        new
                        {
                            vehicle.Id,
                            DriverId = driver.Id,
                            vehicle.VehicleType,
                            vehicle.Model,
                            vehicle.RegistrationNumber,
                            vehicle.Color
                        },
                        transaction);
                }
            }

            transaction.Commit();

            // Set partition key for compatibility
            driver.Pk = $"Driver:{driver.Id.ToLower()}";

            return driver;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> Delete(string id)
    {
        using var connection = SqlConnectionBuilder.GetConnection(_configuration);
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // Check if driver exists
            var exists = await connection.ExecuteScalarAsync<bool>(
                "SELECT COUNT(1) FROM Drivers WHERE Id = @Id",
                new { Id = id },
                transaction);

            if (!exists)
            {
                return false;
            }

            // Delete vehicles first (foreign key constraint)
            await connection.ExecuteAsync(
                "DELETE FROM DriverVehicles WHERE DriverId = @DriverId",
                new { DriverId = id },
                transaction);

            // Delete driver
            await connection.ExecuteAsync(
                "DELETE FROM Drivers WHERE Id = @Id",
                new { Id = id },
                transaction);

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
