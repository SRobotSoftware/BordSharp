using API.Domain.Models;
using FileContextCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace bord
{
    public static class Defaults
    {
        public static string DefaultBoardName => "Tasks";
        public static string DefaultTaskDescription => "Hello World";
        public static int DefaultTaskPriority => 1;
        public static EPriority DefaultTaskEPriority => EPriority.low;
    }
    public static class DataContexts
    {
        public static DbContextOptions<BordsContext> Remote => new DbContextOptionsBuilder<BordsContext>()
            .UseNpgsql("Host=somehost;Port=0;Database=defaultdb;Username=someuser;Password=somepassword;SslMode=Require;TrustServerCertificate=true")
            .Options;
        public static DbContextOptions<BordsContext> Local => new DbContextOptionsBuilder<BordsContext>()
            .UseFileContextDatabase(location: @"C:\Users\Public\Bord")
            .Options;
        public static DbContextOptions<BordsContext> InMemory => new DbContextOptionsBuilder<BordsContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }
    public class BadTaskIdException : Exception
    {
        public BadTaskIdException() { }
        public BadTaskIdException(string message) : base(message) { }
        public BadTaskIdException(string message, Exception inner) : base(message, inner) { }
    }
    public class PriorityOutOfBoundsException : Exception
    {
        public PriorityOutOfBoundsException() { }
        public PriorityOutOfBoundsException(string message) : base(message) { }
        public PriorityOutOfBoundsException(string message, Exception inner) : base(message, inner) { }
    }
}
