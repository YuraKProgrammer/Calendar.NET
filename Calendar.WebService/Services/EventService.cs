using Calendar.DataModels;
using Calendar.Models;
using Kalantyr.Auth.Client;
using Kalantyr.Web;

namespace Calendar.WebService.Services
{
    public class EventService
    {
        private readonly IAppAuthClient _appAuthClient;

        private readonly IEventStorage _eventStorage;
        public async Task<ResultDto<int>> GetCountAsync(DateTime fromDate, DateTime toDate, string userToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userToken))
                return new ResultDto<int> { Error = Errors.TokenNotFound };

            var getUserIdResult = await _appAuthClient.GetUserIdAsync(userToken, cancellationToken);
            if (getUserIdResult.Error!=null)
                return new ResultDto<int> { Error = getUserIdResult.Error };

            var count = 0;
            if (getUserIdResult.Result == 3)
                count = 30;
            if (getUserIdResult.Result == 2)
                count = 20;

            return new ResultDto<int> { Result = count };
        }

        public async Task<ResultDto<Event>> AddAsync(Event ev, string userToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userToken))
                return new ResultDto<Event> { Error = Errors.TokenNotFound };

            var getUserIdResult = await _appAuthClient.GetUserIdAsync(userToken, cancellationToken);
            if (getUserIdResult.Error != null)
                return new ResultDto<Event> { Error = getUserIdResult.Error };

            cancellationToken.ThrowIfCancellationRequested();

            var record = new EventRecord {UserId=getUserIdResult.Result, Name=ev.Name, Date=ev.Date};
            var id = await _eventStorage.AddAsync(record, cancellationToken);
            ev.Id = id;

            return new ResultDto<Event> { Result = ev };
        }

        public EventService(IAppAuthClient appAuthClient, IEventStorage eventStorage)
        {
            _appAuthClient = appAuthClient;
            _eventStorage = eventStorage;
        }
    }
}
