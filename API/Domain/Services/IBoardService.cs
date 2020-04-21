using API.Domain.Services.Communication;
using API.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Domain.Services
{
    public interface IBoardService
    {
        Task<IEnumerable<Models.Board>> ListAsync();
        Task<Board> FindAsync(int id);
        Task<BoardResponse> SaveAsync(Board board);
        Task<BoardResponse> UpdateAsync(int id, Board board);
        Task<BoardResponse> DeleteAsync(int id);
        Task<Board> FindWithTasksAsync(int id);
    }
}
