using System.Collections.Generic;
using System.Threading.Tasks;

namespace Taxi.Core.Infrastructure
{
    public interface IPassengerRepository
    {
        Task<IEnumerable<Domain.Passenger>> GetAll();

        Task<Domain.Passenger> GetById(string id);

        Task<string> Create(Domain.Passenger passenger);

        Task<Domain.Passenger> Update(string id, Domain.Passenger passenger);

        Task<bool> Delete(string id);
    }
}
