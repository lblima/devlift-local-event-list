using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevLiftLocalEventList.Domain.Interfaces
{
    public interface IEventTypeRepository
    {
        Task<List<EventType>> GetAll();
        Task<EventType> GetOne(long id);
        void Add(EventType newEventType);
        void Remove(EventType removedEventType);
        Task SaveChanges();
    }
}