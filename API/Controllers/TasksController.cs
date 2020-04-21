using API.Domain.Services;
using API.Extensions;
using API.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        public TasksController(ITaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<TaskMinimalResource>> ListAsync()
        {
            var tasks = await _taskService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Domain.Models.Task>, IEnumerable<TaskMinimalResource>>(tasks);
            return resources;
        }
        [HttpGet("{id}")]
        public async Task<TaskResource> FindAsync(int id)
        {
            var board = await _taskService.FindAsync(id);
            var resource = _mapper.Map<Domain.Models.Task, TaskResource>(board);
            return resource;
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveTaskResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var task = _mapper.Map<SaveTaskResource, Domain.Models.Task>(resource);
            var result = await _taskService.SaveAsync(task);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            var taskResource = _mapper.Map<Domain.Models.Task, TaskMinimalResource>(result.Task);
            return Ok(taskResource);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveTaskResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var task = _mapper.Map<SaveTaskResource, Domain.Models.Task>(resource);
            var result = await _taskService.UpdateAsync(id, task);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var taskResource = _mapper.Map<Domain.Models.Task, TaskMinimalResource>(result.Task);
            return Ok(taskResource);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _taskService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var taskResource = _mapper.Map<Domain.Models.Task, TaskMinimalResource>(result.Task);
            return Ok(taskResource);
        }
    }
}
