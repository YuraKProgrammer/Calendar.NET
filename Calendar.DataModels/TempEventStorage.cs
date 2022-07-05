using Calendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calendar.DataModels
{
    public class TempEventStorage : IEventStorage
    {
        private static readonly ICollection<EventRecord> _events = new List<EventRecord>();

        private uint _eventId = 0;
        public async Task<uint> AddAsync(EventRecord eventRecord, CancellationToken cancellationToken)
        {
            eventRecord.Id = _eventId;
            _events.Add(eventRecord);
            _eventId++;
            return eventRecord.Id;
        }

        public async Task DeleteEventAsync(uint id, CancellationToken cancellationToken)
        {
            var er = _events.FirstOrDefault(e => e.Id == id);
            if (er!=null)
                _events.Remove(er);
        }

        public async Task EditAsync(EventRecord eventRecord, CancellationToken cancellationToken)
        {
            var er = _events.FirstOrDefault(e => e.Id == eventRecord.Id);
            if (er != null)
            {
                _events.Remove(er);
                _events.Add(eventRecord);
            }
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
