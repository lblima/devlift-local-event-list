using DevLiftLocalEventList.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevLiftLocalEventList.Infrastructure
{
    public class LocalEventContext: DbContext
    {
        public LocalEventContext(DbContextOptions<LocalEventContext> options)
           : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
    }
}