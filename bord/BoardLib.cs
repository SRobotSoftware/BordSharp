using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bord
{
    public class BoardLib
    {
        private BordsContext DB { get; set; }
        public BoardLib(BordsContext db) { DB = db; }
        public Task GetTask(int taskId) => DB.Tasks.Where(t => t.TaskId == taskId).SingleOrDefault();
        public Task CreateTask(string description, int priority = 1)
        {
            ValidatePriority(priority);

            Board board = DB.Boards.Where(b => b.Name == Defaults.DefaultBoardName).SingleOrDefault();

            if (board == null) board = GetBoard(Defaults.DefaultBoardName);

            Task task = new Task { BoardId = board.BoardId, Description = description, Priority = priority };

            DB.Tasks.Add(task);
            DB.SaveChanges();

            return task;
        }
        public Task CreateTask(string boardName, string description, int priority = 1)
        {
            ValidatePriority(priority);

            Board board = GetBoard(boardName);
            Task task = new Task { BoardId = board.BoardId, Description = description, Priority = priority };

            DB.Tasks.Add(task);
            DB.SaveChanges();

            return task;
        }
        public Board GetBoard(int boardId)
        {
            Board board = DB.Boards.Where(b => b.BoardId == boardId).Include(b => b.Tasks).FirstOrDefault();

            if (board == null) board = GetBoard(Defaults.DefaultBoardName);

            return board;
        }
        public Board GetBoard(string name)
        {
            string nameLower = name.ToLower();
            Board board = DB.Boards.Where(b => b.Name.ToLower() == nameLower).Include(b => b.Tasks).FirstOrDefault();

            if (board == null)
            {
                board = new Board { Name = name };

                DB.Boards.Add(board);
                DB.SaveChanges();
            }

            return board;
        }
        public Task ToggleTaskComplete(int taskId)
        {
            Task task = GetTask(taskId);

            if (task == null) throw new BadTaskIdException();

            task.IsCompleted = !task.IsCompleted;

            DB.SaveChanges();

            return task;
        }
        public Task PrioritizeTask(int taskId, int priority)
        {
            ValidatePriority(priority);

            Task task = GetTask(taskId);

            if (task == null) throw new BadTaskIdException();

            task.Priority = priority;

            DB.SaveChanges();

            return task;
        }
        public Task DescribeTask(int taskId, string description)
        {
            Task task = GetTask(taskId);

            if (task == null) throw new BadTaskIdException();

            task.Description = description;

            DB.SaveChanges();

            return task;
        }
        public Task MoveTask(int taskId, string boardName)
        {
            Task task = GetTask(taskId);

            if (task == null) throw new BadTaskIdException();

            Board board = GetBoard(boardName);
            task.BoardId = board.BoardId;

            DB.SaveChanges();

            return task;
        }
        public Task DeleteTask(int taskId)
        {
            Task task = GetTask(taskId);

            if (task == null) throw new BadTaskIdException();

            DB.Tasks.Remove(task);
            DB.SaveChanges();

            return task;
        }
        private bool ValidatePriority(int priority)
        {
            if (priority > 3 || priority < 1) throw new PriorityOutOfBoundsException();

            return true;
        }
        public string GetPrintableTaskIsCompleted(Task task)
        {
            string value = "[ ]";

            if (task.IsCompleted) value = "[X]";

            return value;
        }
        public string GetPrintableTaskPriority(Task task)
        { // GOOGLE: value object
            string value = "  ";

            if (task.Priority >= 3) value = "!!";
            else if (task.Priority == 2) value = "! ";

            return value;
        }
        private int IntLength(int i) // Stole this from https://stackoverflow.com/a/22999111
        {
            if (i < 0) throw new ArgumentOutOfRangeException();
            if (i == 0) return 1;

            return (int)Math.Floor(Math.Log10(i)) + 1;
        }
        public int GetHighestTaskId(BordsContext db)
        {
            Task task = db.Tasks.OrderByDescending(t => t.TaskId).FirstOrDefault();

            if (task == null)
            {
                task = new Task { TaskId = 1 };
            }

            return task.TaskId;
        }
        public List<Task> GetPendingTasks(Board board) => board.Tasks.Where(t => t.IsCompleted == false).OrderBy(t => t.TaskId).ToList();
        public List<Task> GetCompletedTasks(Board board) => board.Tasks.Where(t => t.IsCompleted == true).OrderBy(t => t.TaskId).ToList();
        public void PrintTask(Task task, int padAmount = 2)
        {
            if (task.IsCompleted) Console.ForegroundColor = ConsoleColor.Green;
            else if (task.Priority == 1) Console.ForegroundColor = ConsoleColor.Blue;
            else if (task.Priority == 2) Console.ForegroundColor = ConsoleColor.Yellow;
            else if (task.Priority >= 3) Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(
                "{0} {1} {2} {3}",
                task.TaskId.ToString().PadLeft(padAmount, '0'),
                GetPrintableTaskPriority(task),
                GetPrintableTaskIsCompleted(task),
                task.Description);
        }
        public void PrintBoard(Board board, int padAmount)
        {
            List<Task> pendingTasks = GetPendingTasks(board);
            List<Task> completedTasks = GetCompletedTasks(board);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("@{0}", board.Name);
            Console.ResetColor();

            foreach (Task task in pendingTasks) PrintTask(task, padAmount);

            if (completedTasks.Count() >= 1)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("---");
                Console.ResetColor();
                foreach (Task task in completedTasks) PrintTask(task, padAmount);
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Completed Tasks: {0} | Pending: {1} | Total {2}", completedTasks.Count(), pendingTasks.Count(), board.Tasks.Count());
            Console.ResetColor();
        }
        public void PrintAll()
        {
            List<Board> boards = DB.Boards.Include(b => b.Tasks).OrderBy(b => b.BoardId).ToList();
            int padAmount = IntLength(GetHighestTaskId(DB));

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Bord");
            Console.WriteLine("---");
            Console.ResetColor();

            foreach (Board board in boards) PrintBoard(board, padAmount);
        }
        public void CleanupEmptyBoards()
        {
            List<Board> boards = DB.Boards.Include(b => b.Tasks).ToList();

            foreach (Board board in boards)
            {
                if (board.Name != Defaults.DefaultBoardName && board.Tasks.Count() == 0) DB.Boards.Remove(board);
            }

            DB.SaveChanges();
        }
    }
}
