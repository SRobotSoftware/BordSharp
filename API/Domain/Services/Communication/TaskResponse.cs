using API.Domain.Models;

namespace API.Domain.Services.Communication
{
    public class TaskResponse : BaseResponse
    {
        public Task Task { get; private set; }
        private TaskResponse(bool success, string message, Task task) : base(success, message)
        {
            Task = task;
        }
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="board">Saved task.</param>
        /// <returns>Response.</returns>
        public TaskResponse(Task task) : this(true, string.Empty, task)
        {

        }
        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public TaskResponse(string message) : this(false, message, null)
        {

        }
    }
}
