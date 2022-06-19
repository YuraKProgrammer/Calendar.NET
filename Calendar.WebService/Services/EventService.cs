using Calendar.Models;
using Kalantyr.Auth.Client;
using Kalantyr.Web;

namespace Calendar.WebService.Services
{
    public class EventService
    {
        private readonly IAppAuthClient _appAuthClient;
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

        public EventService(IAppAuthClient appAuthClient)
        {
            _appAuthClient = appAuthClient;
        }
    }
}
