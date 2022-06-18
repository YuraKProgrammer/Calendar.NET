using Kalantyr.Web;

namespace Calendar.WebService.Services
{
    public class EventService
    {
        public ResultDto<int> GetCount(DateTime fromDate, DateTime toDate, string userToken)
        {
            return new ResultDto<int> { Result = 5 };
        }
    }
}
