﻿using DevLiftLocalEventList.Domain;
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

        public EventController(IEventRepository eventRepository, IEventTypeRepository eventTypeRepository)
        {
            _eventRepository = eventRepository;
            _eventTypeRepository = eventTypeRepository;
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