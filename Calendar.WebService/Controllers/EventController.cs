using Calendar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        [HttpGet]
        [Route("Count")]
        public IActionResult GetCount(DateTime fromDate, DateTime toDate)
        {
            return Ok(5);
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
    }
}
