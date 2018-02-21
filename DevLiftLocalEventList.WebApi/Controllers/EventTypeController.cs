using AutoMapper;
using DevLiftLocalEventList.Domain;
using DevLiftLocalEventList.Domain.Interfaces;
using DevLiftLocalEventList.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevLiftLocalEventList.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class EventTypeController : Controller
    {
        private readonly IEventTypeRepository _eventTypeRepository;
        private readonly IMapper _mapper;

        public EventTypeController(IEventTypeRepository eventTypeRepository, IMapper mapper)
        {
            _eventTypeRepository = eventTypeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<EventTypeViewModel> Get()
        {
            return _mapper.Map<List<EventType>, List<EventTypeViewModel>>(_eventTypeRepository.GetAll().Result);
        }

        [HttpGet("{id}", Name = "GetEventType")]
        public IActionResult Get(int id)
        {
            var _eventType = _eventTypeRepository.GetOne(id).Result;
            if (_eventType == null)
            {
                return NotFound();
            }

            return new ObjectResult(_mapper.Map<EventType, EventTypeViewModel>(_eventType));
        }

        [HttpPost]
        public IActionResult Post([FromBody]EventTypeViewModel posteEventType)
        {
            if (posteEventType == null)
            {
                ModelState.AddModelError("EventType", "Check all required fields");
                return BadRequest(ModelState);
            }

            var newEventType = new EventType(posteEventType.Description);

            _eventTypeRepository.Add(newEventType);
            _eventTypeRepository.SaveChanges();

            return CreatedAtRoute("GetEventType", new { id = newEventType.Id }, _mapper.Map<EventType, EventTypeViewModel>(newEventType));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]EventTypeViewModel updatedEventType)
        {
            if (updatedEventType == null || updatedEventType.Id != id)
            {
                ModelState.AddModelError("EventType", "Check all required fields");
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