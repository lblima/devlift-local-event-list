using AutoMapper;
using DevLiftLocalEventList.Domain;
using DevLiftLocalEventList.Domain.Interfaces;
using DevLiftLocalEventList.WebApi.ViewModels;
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
        public IEnumerable<EventViewModel> Get()
        {
            return _mapper.Map<List<Event>, List<EventViewModel>>(_eventRepository.GetAll().Result);
        }

        [HttpGet("{id}", Name = "GetEvent")]
        public IActionResult Get(int id)
        {
            var _event = _eventRepository.GetOne(id).Result;
            if (_event == null)
            {
                return NotFound();
            }

            return new ObjectResult(_mapper.Map<Event, EventViewModel>(_event));
        }

        [HttpPost]
        public IActionResult Post([FromBody]EventViewModel postedEvent)
        {
            if (postedEvent == null)
            {
                ModelState.AddModelError("Event", "Check all required fields");
                return BadRequest(ModelState);
            }

            if (postedEvent.TypeId == 0)
            {
                ModelState.AddModelError("EventType", "You must provide the TypeId");
                return BadRequest(ModelState);
            }

            var eventType = _eventTypeRepository.GetOne(postedEvent.TypeId);

            if (eventType.IsCompleted && eventType.Result == null)
            {
                ModelState.AddModelError("EventType", "The Event Type provided does not exists");
                return BadRequest(ModelState);
            }

            var newEvent = new Event(postedEvent.Description, postedEvent.Summary, postedEvent.Date, eventType.Result);

            _eventRepository.Add(newEvent);
            _eventRepository.SaveChanges();

            return CreatedAtRoute("GetEvent", new { id = newEvent.Id }, _mapper.Map<Event, EventViewModel>(newEvent));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]EventViewModel updatedEvent)
        {
            if (updatedEvent == null)
            {
                ModelState.AddModelError("Event", "Check all required fields");
                return BadRequest(ModelState);
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

            if (updatedEvent.TypeId != 0)
            {
                var eventType = _eventTypeRepository.GetOne(updatedEvent.TypeId);
                if (eventType.IsCompletedSuccessfully && eventType.Result != null)
                {
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