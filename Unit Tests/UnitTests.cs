using API.Domain.Models;
using bord;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language;
using System;

namespace Unit_Tests
{
    [TestClass]
    public class BoardLibShould
    {
        private BoardLib boardLib;
        private BordsContext ctx;
        private Mock<BoardLib> mock;

        [TestInitialize]
        public void Setup()
        {
            ctx = new BordsContext(DataContexts.InMemory);
            boardLib = new BoardLib(ctx);

            mock = new Mock<BoardLib>(ctx);
        }

        [TestCleanup]
        public void TearDown()
        {
            ctx.Database.EnsureDeleted();
        }

        [TestMethod]
        public void NewTask_NoInput_CreatesDefaultTask()
        {
            Task _task = boardLib.CreateTask(Defaults.DefaultTaskDescription, Defaults.DefaultTaskPriority);
            Assert.IsTrue(_task.Description == Defaults.DefaultTaskDescription);
            Assert.IsFalse(_task.IsCompleted);
            Assert.IsTrue(_task.Priority == Defaults.DefaultTaskEPriority);
            Assert.IsTrue(_task.Board.Name == Defaults.DefaultBoardName);
        }
        [TestMethod]
        public void NewTask_PriorityOnly_CreatesPrioritizedDefaultTask()
        {
            Task _task = boardLib.CreateTask(Defaults.DefaultTaskDescription, 3);
            Assert.IsTrue(_task.Description == Defaults.DefaultTaskDescription);
            Assert.IsFalse(_task.IsCompleted);
            Assert.IsTrue(_task.Priority == EPriority.high);
            Assert.IsTrue(_task.Board.Name == Defaults.DefaultBoardName);
        }
        [TestMethod]
        public void NewTask_CustomBoard_CreatesNewTaskOnNewBoard()
        {
            string boardname = "TestBoard";
            Task _task = boardLib.CreateTask(boardname, Defaults.DefaultTaskDescription, 1);
            Assert.IsTrue(_task.Description == Defaults.DefaultTaskDescription);
            Assert.IsFalse(_task.IsCompleted);
            Assert.IsTrue(_task.Priority == Defaults.DefaultTaskEPriority);
            Assert.IsTrue(_task.Board.Name == boardname);
        }
        [TestMethod]
        public void NewTask_CustomDescription_CreatesNewTaskWithDescription()
        {
            string description = "Test Description";
            Task _task = boardLib.CreateTask(description, Defaults.DefaultTaskPriority);
            Assert.IsTrue(_task.Description == description);
            Assert.IsFalse(_task.IsCompleted);
            Assert.IsTrue(_task.Priority == Defaults.DefaultTaskEPriority);
            Assert.IsTrue(_task.Board.Name == Defaults.DefaultBoardName);
        }
        [TestMethod]
        public void NewTask_BadPriority()
        {
            Assert.ThrowsException<PriorityOutOfBoundsException>(() => boardLib.CreateTask("", 0));
            Assert.ThrowsException<PriorityOutOfBoundsException>(() => boardLib.CreateTask("", 500));
        }
        [TestMethod]
        public void ToggleTaskComplete_Toggles()
        {
            Task _task = boardLib.CreateTask(Defaults.DefaultTaskDescription, Defaults.DefaultTaskPriority);
            Assert.IsFalse(_task.IsCompleted);
            boardLib.ToggleTaskComplete(_task.TaskId);
            Assert.IsTrue(_task.IsCompleted);
            boardLib.ToggleTaskComplete(_task.TaskId);
            Assert.IsFalse(_task.IsCompleted);
        }
        [TestMethod]
        public void ToggleTaskComplete_ToggleNull() => Assert.ThrowsException<BadTaskIdException>(() => boardLib.ToggleTaskComplete(0));
        [TestMethod]
        public void PrioritizeTask_PrioritizeNull() => Assert.ThrowsException<BadTaskIdException>(() => boardLib.PrioritizeTask(0, 1));
        [TestMethod]
        public void PrioritizeTask_PrioritizeOutOfBounds()
        {
            Task _task = boardLib.CreateTask(Defaults.DefaultTaskDescription, Defaults.DefaultTaskPriority);
            Assert.ThrowsException<PriorityOutOfBoundsException>(() => boardLib.PrioritizeTask(_task.TaskId, 500));
            Assert.ThrowsException<PriorityOutOfBoundsException>(() => boardLib.PrioritizeTask(_task.TaskId, 0));
        }
        [TestMethod]
        public void PrioritizeTask_PrioritizesTask()
        {
            Task _task = boardLib.CreateTask(Defaults.DefaultTaskDescription, Defaults.DefaultTaskPriority);
            boardLib.PrioritizeTask(_task.TaskId, 3);
            Assert.IsTrue(_task.Priority == EPriority.high);
        }
        [TestMethod]
        public void DescribeTask_DescribesTask()
        {
            string description = "Test Description";
            Task _task = boardLib.CreateTask(Defaults.DefaultTaskDescription, Defaults.DefaultTaskPriority);
            boardLib.DescribeTask(_task.TaskId, description);
            Assert.IsTrue(_task.Description == description);
        }
        [TestMethod]
        public void MoveTask_MovesTask()
        {
            Task _task = boardLib.CreateTask(Defaults.DefaultTaskDescription, Defaults.DefaultTaskPriority);
            string boardname = "TestBoard";
            boardLib.MoveTask(_task.TaskId, boardname);
            Assert.IsTrue(_task.Board.Name == boardname);
            Assert.IsTrue(boardLib.GetBoard(boardname).Tasks.Count == 1);
        }
        [TestMethod]
        public void DeleteTask_DeletesTask()
        {
            Task _task = boardLib.CreateTask(Defaults.DefaultTaskDescription, Defaults.DefaultTaskPriority);
            Assert.IsTrue(boardLib.GetBoard(Defaults.DefaultBoardName).Tasks.Count == 1);
            boardLib.DeleteTask(_task.TaskId);
            Assert.IsTrue(boardLib.GetBoard(Defaults.DefaultBoardName).Tasks.Count == 0);
        }
        [TestMethod]
        public void FindBoard_FindsDefault()
        {
            Board board = boardLib.GetBoard(Defaults.DefaultBoardName);
            Assert.IsTrue(board.Name == Defaults.DefaultBoardName);
        }
        [TestMethod]
        public void FindBoard_CreatesIfNotExists()
        {
            string boardname = "Test Board";
            Board board = boardLib.GetBoard(boardname);
            Assert.IsTrue(board.Name == boardname);
        }
        [TestMethod]
        public void FindTask_FindsTask()
        {
            Task _task = boardLib.CreateTask(Defaults.DefaultTaskDescription, Defaults.DefaultTaskPriority);
            Task _task_2 = boardLib.GetTask(_task.TaskId);
            Assert.AreSame(_task, _task_2);
        }
        [TestMethod]
        public void FindTask_FindsNull()
        {
            var x = boardLib.GetTask(0);
            Assert.IsNull(x);
        }
        [TestMethod]
        [Ignore]
        public void PrintAll_CallsChildPrintMethods()
        {
            mock.Object.CreateTask(Defaults.DefaultTaskDescription, Defaults.DefaultTaskPriority);
            mock.Object.CreateTask("Test Board", Defaults.DefaultTaskDescription, Defaults.DefaultTaskPriority);
            mock.Object.PrintAll();
            mock.Verify(m => m.PrintAll(), Times.Exactly(1));
            mock.Verify(m => m.PrintBoard(It.IsAny<Board>(), It.IsAny<int>()), Times.Exactly(2));
            mock.Verify(m => m.PrintTask(It.IsAny<Task>(), It.IsAny<int>()), Times.Exactly(3));
        }
        // Maybe MOQ for testing Print?
    }
}
