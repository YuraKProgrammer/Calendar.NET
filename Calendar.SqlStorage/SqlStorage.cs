using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Calendar.DataModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

namespace Calendar.SqlStorage
{
    public class SqlStorage: IEventStorage, IHealthCheck, IStorageTools
    {
        private readonly string _connectionString;

        public SqlStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("EventsDB");
        }

        public async Task<uint> AddAsync(EventRecord eventRecord, CancellationToken cancellationToken)
        {
            await using var ctx = new EventsDbContext(_connectionString);
            await ctx.Events.AddAsync(eventRecord, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);
            return eventRecord.Id;
        }

        public async Task EditAsync(EventRecord eventRecord, CancellationToken cancellationToken)
        {
            await using var ctx = new EventsDbContext(_connectionString);
            var record = await ctx.Events.Where(e => e.Id == eventRecord.Id).FirstOrDefaultAsync(cancellationToken);
            if (record == null)
                throw new Exception("Такое событие не найдено");
            cancellationToken.ThrowIfCancellationRequested();
            record.Name = eventRecord.Name;
            record.Date = eventRecord.Date;
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteEventAsync(uint id, CancellationToken cancellationToken)
        {
            await using var ctx = new EventsDbContext(_connectionString);
            var record = await ctx.Events.Where(e => e.Id == id).FirstOrDefaultAsync(cancellationToken);
            if (record != null) {
                cancellationToken.ThrowIfCancellationRequested();
                ctx.Events.Remove(record);
                await ctx.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IReadOnlyCollection<EventRecord>> GetEventsAsync(uint userId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            await using var ctx = new EventsDbContext(_connectionString);
            var records = await ctx.Events
                .Where(e => e.UserId == userId)
                .Where(e => e.Date >= fromDate)
                .Where(e => e.Date <= toDate)
                .ToArrayAsync(cancellationToken);
            return records;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            try
            {
                await using var ctx = new EventsDbContext(_connectionString);
                await ctx.Events.FirstOrDefaultAsync(cancellationToken);
                return HealthCheckResult.Healthy();
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy(nameof(SqlStorage), e);
            }
        }

        public async Task MigrateAsync(CancellationToken cancellationToken)
        {
            await using var ctx = new EventsDbContext(_connectionString);
            await ctx.Database.MigrateAsync(cancellationToken);
        }
    }
}
