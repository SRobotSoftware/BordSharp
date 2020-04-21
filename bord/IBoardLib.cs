using System.Collections.Generic;

namespace bord
{
    public interface IBoardLib
    {
        void CleanupEmptyBoards();
        Task CreateTask(string description, int priority);
        Task CreateTask(string boardName, string description, int priority);
        Task DeleteTask(int taskId);
        Task DescribeTask(int taskId, string description);
        Board GetBoard(int boardId);
        Board GetBoard(string name);
        List<Task> GetCompletedTasks(Board board);
        int GetHighestTaskId(BordsContext db);
        List<Task> GetPendingTasks(Board board);
        string GetPrintableTaskIsCompleted(Task task);
        string GetPrintableTaskPriority(Task task);
        Task GetTask(int taskId);
        Task MoveTask(int taskId, string boardName);
        void PrintAll();
        void PrintBoard(Board board, int padAmount);
        void PrintTask(Task task, int padAmount);
        Task PrioritizeTask(int taskId, int priority);
        Task ToggleTaskComplete(int taskId);
    }
}