using System.Collections.Generic;
using System.Threading.Tasks;

namespace Taxi.Core.Infrastructure
{
    public interface IPassengerRepository
    {
        Task<IEnumerable<Domain.Passenger>> GetAll();

        Task<Domain.Passenger> GetById(string id);

        Task<string> Create(Domain.Passenger driver);

        Task<Domain.Passenger> Update(string id, Domain.Passenger driver);

        Task<bool> Delete(string id);
    }
}
