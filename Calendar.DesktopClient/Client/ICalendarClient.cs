using Calendar.Models;
using Kalantyr.Web;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Calendar.DesktopClient.Client
{
    internal interface ICalendarClient
    {
        Task<ResultDto<int>> GetCountAsync(DateTime fromDate, DateTime toDate, string userToken, CancellationToken cancellationToken);
        Task<ResultDto<Event>> AddAsync(Event ev, string userToken, CancellationToken cancellationToken);
        Task<ResultDto<Event[]>> GetEventsAsync(string userToken, CancellationToken cancellationToken);
    }
}
