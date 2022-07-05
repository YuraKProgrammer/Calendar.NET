using System;
using Calendar.Models;
using Kalantyr.Web;

namespace Calendar.WebService.Services
{
    public class EventValidator : IEventValidator
    {
        public ResultDto<bool> Validate(Event ev)
        {
            if (ev.Date == default || string.IsNullOrWhiteSpace(ev.Name))
                return new ResultDto<bool> { Error = Errors.IncorrectData};
            if (ev.Date <= DateTime.MinValue || ev.Date >= DateTime.MaxValue)
                return new ResultDto<bool> { Error = Errors.IncorrectData };
            if (ev.Name.Length > 30)
                return new ResultDto<bool> { Error = Errors.IncorrectData };
            if (ev.Name.StartsWith('@') || ev.Name.StartsWith('#') || ev.Name.StartsWith('*') || ev.Name.StartsWith('&') || ev.Name.StartsWith('?') || ev.Name.StartsWith('$'))
                return new ResultDto<bool> { Error = Errors.IncorrectData };
            return ResultDto<bool>.Ok;
        }
    }
}
