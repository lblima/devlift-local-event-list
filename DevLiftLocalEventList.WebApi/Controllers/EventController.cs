using AutoMapper;
using DevLiftLocalEventList.Domain;
using DevLiftLocalEventList.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DevLiftLocalEventList.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventTypeRepository _eventTypeRepository;
        private readonly IMapper _mapper;

        public EventController(IEventRepository eventRepository, IEventTypeRepository eventTypeRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _eventTypeRepository = eventTypeRepository;
            _mapper = mapper;

            // TODO Remove before publish
            // Use this block just to create a first fake event test in order to test on POSTMAN (to run the unit tests it isn´t necessary)
            if (_eventRepository.GetAll().Result.Count == 0)
            {
                var eventType = new EventType("Celebration");
                var eventDescription = "My celebration to get hired at DevLift";
                var eventDate = new DateTime(2018, 03, 15);
                var summary = "his event is a party to celebrate this achievement and all my friends are invited to participate. Bring only your good vibes and happiness";

                var newEvent = new Event(eventDescription, summary, eventDate, eventType) { Price = 25 };


                _eventRepository.Add(newEvent);
                _eventRepository.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Event> Get()
        {
            return _eventRepository.GetAll().Result;
        }

        [HttpGet("{id}", Name = "GetEvent")]
        public IActionResult Get(int id)
        {
            var _event = _eventRepository.GetOne(id).Result;
            if (_event == null)
            {
                return NotFound();
            }

            return new ObjectResult(_event);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Event newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest();
            }

            if (newEvent.Type != null && newEvent.Type.Id == 0)
            {
                ModelState.AddModelError("Type", "You must provide the EventType ID { id: x }");
                return BadRequest(ModelState);
            }

            _eventRepository.Add(newEvent);
            _eventRepository.SaveChanges();

            return CreatedAtRoute("GetEvent", new { id = newEvent.Id }, newEvent);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Event updatedEvent)
        {
            if (updatedEvent == null || updatedEvent.Id != id)
            {
                return BadRequest();
            }

            var originalEvent = _eventRepository.GetOne(id).Result;
            if (originalEvent == null)
            {
                return NotFound();
            }

            originalEvent.Date = updatedEvent.Date;
            originalEvent.Description = updatedEvent.Description;
            originalEvent.ImageLink = updatedEvent.ImageLink;
            originalEvent.Price = updatedEvent.Price;
            originalEvent.Summary = updatedEvent.Summary;

            if (updatedEvent.Type != null)
            {
                var eventType = _eventTypeRepository.GetOne(updatedEvent.Type.Id);
                if (eventType.IsCompletedSuccessfully && eventType.Result != null)
                {
                    var originalEventType = eventType.Result;
                    originalEventType.Description = updatedEvent.Type.Description;
                    originalEvent.Type = eventType.Result;
                }
            }

            _eventRepository.SaveChanges();

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var originalEvent = _eventRepository.GetOne(id).Result;

            if (originalEvent == null)
            {
                return NotFound();
            }

            _eventRepository.Remove(originalEvent);
            _eventRepository.SaveChanges();

            return new NoContentResult();
        }
    }
}