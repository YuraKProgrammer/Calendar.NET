using Calendar.Models;
using Kalantyr.Web;

namespace Calendar.WebService.Services
{
    public class EventService
    {
        public async Task<ResultDto<int>> GetCountAsync(DateTime fromDate, DateTime toDate, string userToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userToken))
                return new ResultDto<int> { Error = Errors.TokenNotFound };
            return new ResultDto<int> { Result = 5 };
        }
    }
}
