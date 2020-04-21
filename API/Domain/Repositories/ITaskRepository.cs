using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Models.Task>> ListAsync();
        Task<Models.Task> FindByIdAsync(int id);
        Task AddAsync(Models.Task task);
        void Update(Models.Task task);
        void Remove(Models.Task task);
    }
}
