using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Calendar.DataModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Calendar.SqlStorage
{
    public class SqlStorage: IEventStorage, IHealthCheck
    {
        private readonly string _connectionString;

        public SqlStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("EventsDB");
        }

        public async Task<uint> AddAsync(EventRecord eventRecord, CancellationToken cancellationToken)
        {
            await using var ctx = new EventsDbContext(_connectionString);
            throw new NotImplementedException();
        }

        public async Task EditAsync(EventRecord eventRecord, CancellationToken cancellationToken)
        {
            await using var ctx = new EventsDbContext(_connectionString);
            throw new NotImplementedException();
        }

        public async Task DeleteEventAsync(uint id, CancellationToken cancellationToken)
        {
            await using var ctx = new EventsDbContext(_connectionString);
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<EventRecord>> GetEventsAsync(uint userId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            await using var ctx = new EventsDbContext(_connectionString);
            throw new NotImplementedException();
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            try
            {
                await using var ctx = new EventsDbContext(_connectionString);
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy(nameof(SqlStorage), e);
            }
        }
    }
}
