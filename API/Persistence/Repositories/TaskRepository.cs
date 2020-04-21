using API.Domain.Repositories;
using API.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Persistence.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context)
        {

        }
        public async Task AddAsync(Domain.Models.Task task)
        {
            await _context.Tasks.AddAsync(task);
        }
        public async Task<Domain.Models.Task> FindByIdAsync(int id)
        {
            return await _context.Tasks.Include(p => p.Board).Where(m => m.BoardId == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Domain.Models.Task>> ListAsync()
        {
            return await _context.Tasks.Include(p => p.Board)
                .ToListAsync();
        }
        public void Remove(Domain.Models.Task task)
        {
            _context.Tasks.Remove(task);
        }
        public void Update(Domain.Models.Task task)
        {
            _context.Tasks.Update(task);
        }
    }
}
