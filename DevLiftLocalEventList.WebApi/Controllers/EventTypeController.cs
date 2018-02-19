using DevLiftLocalEventList.Domain;
using DevLiftLocalEventList.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevLiftLocalEventList.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class EventTypeController : Controller
    {
        private readonly IEventTypeRepository _eventTypeRepository;

        public EventTypeController(IEventTypeRepository eventTypeRepository)
        {
            _eventTypeRepository = eventTypeRepository;
        }

        [HttpGet]
        public IEnumerable<EventType> Get()
        {
            return _eventTypeRepository.GetAll().Result;
        }

        [HttpGet("{id}", Name = "GetEventType")]
        public IActionResult Get(int id)
        {
            var _eventType = _eventTypeRepository.GetOne(id).Result;
            if (_eventType == null)
            {
                return NotFound();
            }

            return new ObjectResult(_eventType);
        }

        [HttpPost]
        public IActionResult Post([FromBody]EventType newEventType)
        {
            if (newEventType == null)
            {
                return BadRequest();
            }

            _eventTypeRepository.Add(newEventType);
            _eventTypeRepository.SaveChanges();

            return CreatedAtRoute("GetEventType", new { id = newEventType.Id }, newEventType);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]EventType updatedEventType)
        {
            if (updatedEventType == null || updatedEventType.Id != id)
            {
                return BadRequest();
            }

            var originalEventType = _eventTypeRepository.GetOne(id).Result;
            if (originalEventType == null)
            {
                return NotFound();
            }

            originalEventType.Description = updatedEventType.Description;

            _eventTypeRepository.SaveChanges();

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var originalEventType = _eventTypeRepository.GetOne(id).Result;

            if (originalEventType == null)
            {
                return NotFound();
            }

            _eventTypeRepository.Remove(originalEventType);
            _eventTypeRepository.SaveChanges();

            return new NoContentResult();
        }
    }
}