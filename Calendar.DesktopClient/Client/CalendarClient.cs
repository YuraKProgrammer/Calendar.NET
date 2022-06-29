using Calendar.Models;
using Kalantyr.Web;
using Kalantyr.Web.Impl;
using System;
using System.Collections.Generic;
using System.Net;
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

        public async Task<ResultDto<bool>> DeleteEventAsync(Event ev, string userToken, CancellationToken cancellationToken)
        {
            _enricher.Token = userToken;
            return await Post<ResultDto<bool>>($"/event/delete?id={ev.Id}", null, cancellationToken);
        }

        public async Task<ResultDto<Event>> EditAsync(Event ev, string userToken, CancellationToken cancellationToken)
        {
            _enricher.Token = userToken;
            return await Post<ResultDto<Event>>($"/event/edit", Serialize(ev), cancellationToken);
        }

        public async Task<ResultDto<int>> GetCountAsync(DateTime fromDate, DateTime toDate, string userToken, CancellationToken cancellationToken)
        {
            _enricher.Token = userToken;
            var f = WebUtility.UrlEncode(fromDate.ToString());
            var t = WebUtility.UrlEncode(toDate.ToString());
            string path = $"/event/count?fromDate={f}&toDate={t}";
            return await Get<ResultDto<int>>(path, cancellationToken);
        }

        public async Task<ResultDto<Event[]>> GetEventsAsync(string userToken, CancellationToken cancellationToken)
        {
            _enricher.Token = userToken;
            return await Get<ResultDto<Event[]>>("/event/events", cancellationToken);
        }
    }
}
