using DevLiftLocalEventList.Domain;
using DevLiftLocalEventList.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevLiftLocalEventList.Infrastructure.Repositories
{
    public class EventTypeRepository : IEventTypeRepository
    {
        private readonly LocalEventContext _context;

        public EventTypeRepository(LocalEventContext context)
        {
            _context = context;
        }

        public void Add(EventType newEventType) => 
            _context.EventTypes
                .Add(newEventType);

        public Task<List<EventType>> GetAll() =>
            _context.EventTypes
                .ToListAsync();

        public Task<EventType> GetOne(long id) => 
            _context.EventTypes
                .SingleOrDefaultAsync(m => m.Id == id);

        public void Remove(EventType removedEventType) =>
            _context.EventTypes
                .Remove(removedEventType);

        public Task SaveChanges() =>
            _context
                .SaveChangesAsync();
    }
}