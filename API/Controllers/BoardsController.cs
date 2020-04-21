using API.Domain.Models;
using API.Domain.Services;
using API.Resources;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly IMapper _mapper;
        public BoardsController(IBoardService boardService, IMapper mapper)
        {
            _boardService = boardService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<BoardMinimalResource>> ListAsync()
        {
            var boards = await _boardService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Board>, IEnumerable<BoardMinimalResource>>(boards);
            return resources;
        }
        [HttpGet("{id}")]
        public async Task<BoardMinimalResource> FindAsync(int id)
        {
            var board = await _boardService.FindAsync(id);
            var resource = _mapper.Map<Board, BoardMinimalResource>(board);
            return resource;
        }
        [HttpGet("{id}/tasks")]
        public async Task<BoardResource> ListTasksAsync(int id)
        {
            var board = await _boardService.FindWithTasksAsync(id);
            var resource = _mapper.Map<Board, BoardResource>(board);
            return resource;
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveBoardResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var board = _mapper.Map<SaveBoardResource, Board>(resource);
            var result = await _boardService.SaveAsync(board);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            var boardResource = _mapper.Map<Board, BoardMinimalResource>(result.Board);
            return Ok(boardResource);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveBoardResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var board = _mapper.Map<SaveBoardResource, Board>(resource);
            var result = await _boardService.UpdateAsync(id, board);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var boardResource = _mapper.Map<Board, BoardMinimalResource>(result.Board);
            return Ok(boardResource);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _boardService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var boardResource = _mapper.Map<Board, BoardMinimalResource>(result.Board);
            return Ok(boardResource);
        }
    }
}
