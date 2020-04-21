using API.Domain.Repositories;
using API.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Domain.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TaskService(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TaskResponse> DeleteAsync(int id)
        {
            var existingTask = await _taskRepository.FindByIdAsync(id);

            if (existingTask == null)
            {
                return new TaskResponse("Task not found");
            }

            try
            {
                _taskRepository.Remove(existingTask);
                await _unitOfWork.CompleteAsync();

                return new TaskResponse(existingTask);
            }
            catch (Exception ex)
            {
                return new TaskResponse($"An error occurred when deleting the task: {ex.Message}");
            }
        }
        public async Task<Models.Task> FindAsync(int id)
        {
            return await _taskRepository.FindByIdAsync(id);
        }
        public async Task<IEnumerable<Models.Task>> ListAsync()
        {
            return await _taskRepository.ListAsync();
        }
        public async Task<TaskResponse> SaveAsync(Models.Task task)
        {
            try
            {
                await _taskRepository.AddAsync(task);
                await _unitOfWork.CompleteAsync();

                return new TaskResponse(task);
            }
            catch (Exception ex)
            {
                return new TaskResponse($"An error occurred when saving this task: {ex.Message}");
            }
        }
        public async Task<TaskResponse> UpdateAsync(int id, Models.Task task)
        {
            var existingTask = await _taskRepository.FindByIdAsync(id);

            if (existingTask == null)
            {
                return new TaskResponse("Task not found.");
            }

            existingTask.Description = task.Description;
            existingTask.Priority = task.Priority;
            existingTask.IsCompleted = task.IsCompleted;
            existingTask.BoardId = task.BoardId;

            try
            {
                _taskRepository.Update(existingTask);
                await _unitOfWork.CompleteAsync();

                return new TaskResponse(existingTask);
            }
            catch (Exception ex)
            {
                return new TaskResponse($"An error occurred when updating the task: {ex.Message}");
            }
        }
    }
}
