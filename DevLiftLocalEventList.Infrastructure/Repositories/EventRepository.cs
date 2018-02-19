using DevLiftLocalEventList.Domain;
using DevLiftLocalEventList.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevLiftLocalEventList.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly LocalEventContext _context;

        public EventRepository(LocalEventContext context)
        {
            _context = context;
        }

        public void Add(Event newEvent) => 
            _context
                .Events.Add(newEvent);

        public Task<List<Event>> GetAll() =>
            _context.Events
                .Include(x => x.Type).ToListAsync();

        public Task<Event> GetOne(long id) => 
            _context.Events
                .Include(x => x.Type)
                    .SingleOrDefaultAsync(m => m.Id == id);

        public void Remove(Event removedEvent) =>
            _context.Events
                .Remove(removedEvent);

        public Task SaveChanges() =>
            _context
                .SaveChangesAsync();
    }
}