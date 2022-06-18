using Kalantyr.Web;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Calendar.DesktopClient.Client
{
    internal interface ICalendarClient
    {
        Task<ResultDto<int>> GetCountAsync(DateTime fromDate, DateTime toDate, string userToken, CancellationToken cancellationToken);
    }
}
