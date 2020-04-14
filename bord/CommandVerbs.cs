using CommandLine;

namespace bord
{
    class CommandVerbs
    {
        [Verb("list", HelpText = "List Tasks")]
        public class ListOptions { }
        [Verb("task", HelpText = "Add a new Task")]
        public class NewTaskOptions
        {
            [Value(0, Default = "Hello World", Required = true)]
            public string Description { get; set; }
            [Option('b', "board", Default = "Tasks", HelpText = "Board to add new task to")]
            public string Boardname { get; set; }
            [Option('p', "priority", Default = 1, HelpText = "Priority of the task")]
            public int Priority { get; set; }
        }

        public class TaskIdOption
        {
            [Value(0, Required = true, HelpText = "ID of the target Task")]
            public int TaskId { get; set; }
        }

        [Verb("delete", HelpText = "Delete a Task")]
        public class DeleteTaskOptions : TaskIdOption { }
        [Verb("check", HelpText = "Toggle complete on a Task")]
        public class CheckTaskOptions : TaskIdOption { }
        [Verb("move", HelpText = "Move a Task to another board")]
        public class MoveTaskOptions : TaskIdOption
        {
            [Value(1, Required = true, HelpText = "Destination board")]
            public string DestinationBoard { get; set; }
        }
        [Verb("edit", HelpText = "Edit a Task Description")]
        public class EditTaskOptions : TaskIdOption
        {
            [Value(1, Required = true, HelpText = "New Description to save for task")]
            public string NewDescription { get; set; }
        }
        [Verb("prioritize", HelpText = "Edit a Task Priority")]
        public class PrioritizeTaskOptions : TaskIdOption
        {
            [Value(1, Required = true, HelpText = "New Priority for task")]
            public int NewPriority { get; set; }
        }
    }
}
