﻿using Calendar.Models;
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
        public async Task<IActionResult> GetEventsAsync(CancellationToken cancellationToken)
        {
            var events = await eventService.GetEventsAsync(Request.GetAuthToken(), cancellationToken);
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
