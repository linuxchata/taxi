﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Taxi.Core.Infrastructure
{
    public interface IDriverRepository
    {
        Task<IEnumerable<Domain.Driver>> GetAll();

        Task<string> Create(Domain.Driver driver);

        Task Update(Guid id, string state, Domain.Driver driver);

        Task Delete(Guid id, string state);
    }
}