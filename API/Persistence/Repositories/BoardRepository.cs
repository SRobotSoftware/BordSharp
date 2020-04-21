using API.Domain.Models;
using API.Domain.Repositories;
using API.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Persistence.Repositories
{
    public class BoardRepository : BaseRepository, IBoardRepository
    {
        public BoardRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Domain.Models.Board>> ListAsync()
        {
            return await _context.Boards.ToListAsync();
        }
        public async System.Threading.Tasks.Task AddAsync(Board board)
        {
            await _context.Boards.AddAsync(board);
        }
        public async Task<Board> FindByIdAsync(int id)
        {
            return await _context.Boards.FindAsync(id);
        }
        public async Task<Board> FindByIdWithTasksAsync(int id)
        {
            return await _context.Boards.Include(p => p.Tasks).Where(m => m.BoardId == id).FirstOrDefaultAsync();
        }
        public void Update(Board board)
        {
            _context.Boards.Update(board);
        }
        public void Remove(Board board)
        {
            _context.Boards.Remove(board);
        }
    }
}
