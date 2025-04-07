using Dapper;
using Microsoft.Extensions.Configuration;
using Taxi.Core.Infrastructure;
using Taxi.Domain;

namespace Taxi.Repository.Sql;

public sealed class PassengerRepository : IPassengerRepository
{
    private readonly IConfiguration _configuration;

    public PassengerRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<Passenger>> GetAll()
    {
        using var connection = SqlConnectionBuilder.GetConnection(_configuration);

        var passengers = await connection.QueryAsync<Passenger>(@"
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
                CreatedDate, 
                UpdatedDate, 
                'Passenger' AS Type, 
                Version
            FROM Passengers");

        return passengers;
    }

    public async Task<Passenger> GetById(string id)
    {
        using var connection = SqlConnectionBuilder.GetConnection(_configuration);

        var passenger = await connection.QuerySingleOrDefaultAsync<Passenger>(@"
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
                CreatedDate, 
                UpdatedDate, 
                'Passenger' AS Type, 
                Version
            FROM Passengers
            WHERE Id = @Id",
            new { Id = id });

        if (passenger == null)
        {
            return null!;
        }

        // Set partition key for compatibility with CosmosDB implementation
        passenger.Pk = $"Passenger:{passenger.Id.ToLower()}";

        return passenger;
    }

    public async Task<string> Create(Passenger passenger)
    {
        // Generate new ID if not provided
        if (string.IsNullOrEmpty(passenger.Id))
        {
            passenger.Id = Guid.NewGuid().ToString().ToLower();
        }

        passenger.CreatedDate = DateTime.UtcNow;
        passenger.UpdatedDate = passenger.CreatedDate;

        using var connection = SqlConnectionBuilder.GetConnection(_configuration);
        connection.Open();

        await connection.ExecuteAsync(@"
            INSERT INTO Passengers (
                Id, 
                FirstName, 
                LastName, 
                Email, 
                PhoneNumber, 
                Country, 
                State, 
                Rating, 
                IsActive, 
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
                @CreatedDate, 
                @UpdatedDate, 
                @Version
            )",
            passenger);

        return passenger.Id;
    }

    public async Task<Passenger> Update(string id, Passenger passenger)
    {
        passenger.Id = id.ToLower();
        passenger.UpdatedDate = DateTime.UtcNow;

        using var connection = SqlConnectionBuilder.GetConnection(_configuration);
        connection.Open();

        // Check if passenger exists
        var exists = await connection.ExecuteScalarAsync<bool>(
            "SELECT COUNT(1) FROM Passengers WHERE Id = @Id",
            new { Id = id });

        if (!exists)
        {
            return null!;
        }

        // Update passenger
        await connection.ExecuteAsync(@"
            UPDATE Passengers SET
                FirstName = @FirstName,
                LastName = @LastName,
                Email = @Email,
                PhoneNumber = @PhoneNumber,
                Country = @Country,
                State = @State,
                Rating = @Rating,
                IsActive = @IsActive,
                UpdatedDate = @UpdatedDate,
                Version = @Version
            WHERE Id = @Id",
            passenger);

        // Set partition key for compatibility with CosmosDB implementation
        passenger.Pk = $"Passenger:{passenger.Id.ToLower()}";

        return passenger;
    }

    public async Task<bool> Delete(string id)
    {
        using var connection = SqlConnectionBuilder.GetConnection(_configuration);
        connection.Open();

        // Check if passenger exists
        var exists = await connection.ExecuteScalarAsync<bool>(
            "SELECT COUNT(1) FROM Passengers WHERE Id = @Id",
            new { Id = id });

        if (!exists)
        {
            return false;
        }

        // Delete passenger
        await connection.ExecuteAsync(
            "DELETE FROM Passengers WHERE Id = @Id",
            new { Id = id });

        return true;
    }
}
