using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevLiftLocalEventList.Domain.Interfaces
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAll();
        Task<Event> GetOne(long id);
        void Add(Event newEvent);
        void Remove(Event removedEvent);
        Task SaveChanges();
    }
}