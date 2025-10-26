using TioneCqrs.Commands;

namespace TioneCqrs.Tests.Cqrs.Commands
{
    public class TestCommandWithResult : ICommand
    {
        public TestCommandWithResult(string test)
        {
            Test = test;
        }

        public string Test { get; set; }
    }
}