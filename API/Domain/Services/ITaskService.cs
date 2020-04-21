using API.Domain.Services.Communication;
using API.Resources;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Domain.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Models.Task>> ListAsync();
        Task<Models.Task> FindAsync(int id);
        Task<TaskResponse> SaveAsync(Models.Task task);
        Task<TaskResponse> UpdateAsync(int id, Models.Task task);
        Task<TaskResponse> DeleteAsync(int id);
    }
}
