using API.Domain.Models;

namespace API.Domain.Services.Communication
{
    public class BoardResponse : BaseResponse
    {
        public Board Board { get; private set; }
        private BoardResponse(bool success, string message, Board board) : base(success, message)
        {
            Board = board;
        }
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="board">Saved board.</param>
        /// <returns>Response.</returns>
        public BoardResponse(Board board) : this(true, string.Empty, board)
        {

        }
        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public BoardResponse(string message) : this(false, message, null)
        {

        }
    }
}
