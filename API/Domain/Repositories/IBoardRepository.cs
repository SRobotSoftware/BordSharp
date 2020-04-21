using API.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Domain.Repositories
{
    public interface IBoardRepository
    {
        Task<IEnumerable<Models.Board>> ListAsync();
        System.Threading.Tasks.Task AddAsync(Board board);
        Task<Board> FindByIdAsync(int id);
        Task<Board> FindByIdWithTasksAsync(int id);
        void Update(Board board);
        void Remove(Board board);
    }
}
