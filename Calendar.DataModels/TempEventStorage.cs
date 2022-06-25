using Calendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.DataModels
{
    public class TempEventStorage : IEventStorage
    {
        private static readonly ICollection<EventRecord> _events = new List<EventRecord>();
        public async Task<uint> AddAsync(EventRecord eventRecord, CancellationToken cancellationToken)
        {
            _events.Add(eventRecord);
            uint id = (uint)_events.Count;
            eventRecord.Id = id;
            return id;
        }

        public async Task<IReadOnlyCollection<EventRecord>> GetEventsAsync(uint userId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            return _events
                .Where(e => e.UserId==userId)
                .Where(e => e.Date>=fromDate && e.Date<=toDate)
                .ToArray();
        }
    }
}
