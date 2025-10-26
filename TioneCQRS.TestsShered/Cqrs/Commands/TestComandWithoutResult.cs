using TioneCqrs.Commands;

namespace TioneCqrs.Tests.Cqrs.Commands
{
    public class TestCommandWithoutResult : ICommand
    {
        public TestCommandWithoutResult(string test)
        {
            Test = test;
        }

        public string Test { get; set; }
    }
}