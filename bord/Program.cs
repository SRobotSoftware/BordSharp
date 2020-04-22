using API.Domain.Models;
using CommandLine;
using Microsoft.EntityFrameworkCore;
using System;
using static bord.CommandVerbs;

namespace bord
{
    class Program
    {
        // Using EF Core                - for interfacing with data
        //   with: Npgsql               - for default data storage
        //   with: InMemoryDatabase     - for unit testing
        //   with: FileContextCore      - for feature parity
        // Using MSTest                 - for unit testing
        // Using CommandLine            - for parsing command line args
        static int Main(string[] args)
        {
            // Valid contexts are: Remote (PostgreSQL) or Local (local JSON file)
            DbContextOptions<BordsContext> options = DataContexts.Local;

            using BordsContext db = new BordsContext(options);
            BoardLib lib = new BoardLib(db);

            // Parse and perform command
            ParserResult<object> parsedArguments = Parser.Default.ParseArguments<
                ListOptions,
                NewTaskOptions,
                DeleteTaskOptions,
                CheckTaskOptions,
                MoveTaskOptions,
                EditTaskOptions,
                PrioritizeTaskOptions>(args);

            try
            {
                bool GracefullyExecuted = parsedArguments
                    .MapResult(
                        (ListOptions opts) => true,
                        (NewTaskOptions opts) => (
                            (opts.Boardname == null) ? lib.CreateTask(opts.Description, opts.Priority) : lib.CreateTask(opts.Boardname, opts.Description, opts.Priority)
                        ) is Task ? true : false,
                        (DeleteTaskOptions opts) => lib.DeleteTask(opts.TaskId) is Task ? true : false,
                        (CheckTaskOptions opts) => lib.ToggleTaskComplete(opts.TaskId) is Task ? true : false,
                        (MoveTaskOptions opts) => lib.MoveTask(opts.TaskId, opts.DestinationBoard) is Task ? true : false,
                        (EditTaskOptions opts) => lib.DescribeTask(opts.TaskId, opts.NewDescription) is Task ? true : false,
                        (PrioritizeTaskOptions opts) => lib.PrioritizeTask(opts.TaskId, opts.NewPriority) is Task ? true : false,
                        errs => false);

                lib.CleanupEmptyBoards();
                lib.PrintAll();

                return GracefullyExecuted ? 0 : 1;
            }
            catch (Exception ex)
            {
                if (ex is BadTaskIdException) Console.WriteLine("A Task with that ID was not found");
                else if (ex is PriorityOutOfBoundsException) Console.WriteLine("Priority must be an integer of 1, 2, or 3");
                else throw ex;
                return 1;
            }
        }
    }
}