using Calendar.Models;
using Kalantyr.Web;

namespace Calendar.WebService.Services
{
    public interface IEventValidator
    {
        ResultDto<bool> Validate(Event ev);
    }
}
