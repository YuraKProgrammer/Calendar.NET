using Calendar.DataModels;
using Calendar.Models;
using Kalantyr.Auth.Client;
using Kalantyr.Web;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Calendar.WebService.Services
{
    public class EventService : IHealthCheck
    {
        private readonly IAppAuthClient _appAuthClient;

        private readonly IEventStorage _eventStorage;
        private readonly IEventValidator _eventValidator;

        public async Task<ResultDto<int>> GetCountAsync(DateTime fromDate, DateTime toDate, string userToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userToken))
                return new ResultDto<int> { Error = Errors.TokenNotFound };

            var getUserIdResult = await _appAuthClient.GetUserIdAsync(userToken, cancellationToken);
            if (getUserIdResult.Error!=null)
                return new ResultDto<int> { Error = getUserIdResult.Error };

            var events = await _eventStorage.GetEventsAsync(getUserIdResult.Result, fromDate, toDate, cancellationToken);

            return new ResultDto<int> { Result = events.Count };
        }

        public async Task<ResultDto<Event[]>> GetEventsAsync(string userToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userToken))
                return new ResultDto<Event[]> { Error = Errors.TokenNotFound };

            var getUserIdResult = await _appAuthClient.GetUserIdAsync(userToken, cancellationToken);
            if (getUserIdResult.Error != null)
                return new ResultDto<Event[]> { Error = getUserIdResult.Error };

            cancellationToken.ThrowIfCancellationRequested();

            var events = await _eventStorage.GetEventsAsync(getUserIdResult.Result, DateTime.MinValue, DateTime.MaxValue, cancellationToken);
            return new ResultDto<Event[]> { Result = events.Select(a => Map(a)).ToArray()};
        }

        public async Task<ResultDto<Event>> AddAsync(Event ev, string userToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userToken))
                return new ResultDto<Event> { Error = Errors.TokenNotFound };

            var getUserIdResult = await _appAuthClient.GetUserIdAsync(userToken, cancellationToken);
            if (getUserIdResult.Error != null)
                return new ResultDto<Event> { Error = getUserIdResult.Error };

            cancellationToken.ThrowIfCancellationRequested();

            var validationResult = _eventValidator.Validate(ev);
            if (validationResult.Error != null)
                return new ResultDto<Event> { Error = validationResult.Error };

            var record = new EventRecord {UserId=getUserIdResult.Result, Name=ev.Name, Date=ev.Date};
            var id = await _eventStorage.AddAsync(record, cancellationToken);
            ev.Id = id;

            return new ResultDto<Event> { Result = ev };
        }

        public async Task<ResultDto<Event>> EditAsync(Event ev, string userToken, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(userToken))
                return new ResultDto<Event> { Error = Errors.TokenNotFound };

            var getUserIdResult = await _appAuthClient.GetUserIdAsync(userToken, cancellationToken);
            if (getUserIdResult.Error != null)
                return new ResultDto<Event> { Error = getUserIdResult.Error };

            cancellationToken.ThrowIfCancellationRequested();

            var validationResult = _eventValidator.Validate(ev);
            if (validationResult.Error != null)
                return new ResultDto<Event> { Error = validationResult.Error };

            var record = new EventRecord { UserId = getUserIdResult.Result, Name = ev.Name, Date = ev.Date, Id=ev.Id };
            await _eventStorage.EditAsync(record, cancellationToken);

            return new ResultDto<Event> { Result = ev };
        }

        public async Task<ResultDto<bool>> DeleteAsync(uint id, string userToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userToken))
                return new ResultDto<bool> { Error = Errors.TokenNotFound };

            var getUserIdResult = await _appAuthClient.GetUserIdAsync(userToken, cancellationToken);
            if (getUserIdResult.Error != null)
                return new ResultDto<bool> { Error = getUserIdResult.Error };

            cancellationToken.ThrowIfCancellationRequested();

            await _eventStorage.DeleteEventAsync(id, cancellationToken);

            return new ResultDto<bool> { Result = true};
        }

        public EventService(IAppAuthClient appAuthClient, IEventStorage eventStorage, IEventValidator eventValidator)
        {
            _appAuthClient = appAuthClient;
            _eventStorage = eventStorage;
            _eventValidator = eventValidator;
        }

        private static Event Map(EventRecord er)
        {
            var e = new Event { Date = er.Date, Name = er.Name, Id=er.Id };
            return e;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (_appAuthClient is IHealthCheck hc)
                {
                    var result = await hc.CheckHealthAsync(context, cancellationToken);
                    if (result.Status != HealthStatus.Healthy)
                        return result;
                }

                if (_eventStorage is IHealthCheck hc2)
                {
                    var result = await hc2.CheckHealthAsync(context, cancellationToken);
                    if (result.Status != HealthStatus.Healthy)
                        return result;
                }

                return HealthCheckResult.Healthy();
            }
            catch(Exception ex)
            {
                return HealthCheckResult.Unhealthy(nameof(EventService), ex);
            }
        }
    }
}
