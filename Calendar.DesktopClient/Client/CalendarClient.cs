using Calendar.Models;
using Kalantyr.Web;
using Kalantyr.Web.Impl;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Calendar.DesktopClient.Client
{
    internal class CalendarClient : HttpClientBase, ICalendarClient 
    {
        private readonly TokenRequestEnricher _enricher;
        public CalendarClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory, new TokenRequestEnricher())
        {
            _enricher = (TokenRequestEnricher)RequestEnricher;
        }

        public async Task<ResultDto<Event>> AddAsync(Event ev, string userToken, CancellationToken cancellationToken)
        {
            _enricher.Token = userToken;
            return await Post<ResultDto<Event>>($"/event/add", Serialize(ev),cancellationToken);
        }

        public async Task<ResultDto<int>> GetCountAsync(DateTime fromDate, DateTime toDate, string userToken, CancellationToken cancellationToken)
        {
            _enricher.Token = userToken;
            return await Get<ResultDto<int>>($"/event/count?fromDate={fromDate}&toDate={toDate}", cancellationToken);
        }
    }
}
