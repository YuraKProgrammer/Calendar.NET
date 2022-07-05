using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calendar.DataModels
{
    public interface IEventStorage
    {
        /// <summary>
        /// Вовращает id добавленной записи 
        /// </summary>
        Task<uint> AddAsync(EventRecord eventRecord, CancellationToken cancellationToken);
        Task EditAsync(EventRecord eventRecord, CancellationToken cancellationToken);

        Task DeleteEventAsync(uint id, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает события указанного польователя в указанный промежуток времени
        /// </summary>
        Task<IReadOnlyCollection<EventRecord>> GetEventsAsync(uint userId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
    }
}
