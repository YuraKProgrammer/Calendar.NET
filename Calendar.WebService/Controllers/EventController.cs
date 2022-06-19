using Calendar.Models;
using Calendar.WebService.Services;
using Kalantyr.Web;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly EventService eventService;

        [HttpGet]
        [Route("Count")]
        public async Task<IActionResult> GetCountAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            var value = await eventService.GetCountAsync(fromDate, toDate, Request.GetAuthToken(),cancellationToken);
            return base.Ok(value);
        }

        [HttpGet]
        [Route("Events")]
        public IActionResult GetEvents(DateTime fromDate, DateTime toDate)
        {
            var events = new Event[]{
                new Event{Id=3, Name="Событие1", Date=new DateTime(2000,12,10)},
                new Event{Id=1, Name="луаущаувл", Date=new DateTime(2020,11,6)}
            };
            return Ok(events);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddAsync(Event ev, CancellationToken cancellationToken)
        {
            var value = await eventService.AddAsync(ev, Request.GetAuthToken(), cancellationToken);
            return base.Ok(value);
        }

        public EventController(EventService eventService)
        {
            this.eventService = eventService;
        }
    }
}
