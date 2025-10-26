using TioneCqrs.Queries;

namespace TioneCqrs.Tests.Cqrs.Queries
{
    public class TestQuery : IQuery
    {
        public string Test { get; }

        public TestQuery(string test)
        {
            Test = test;
        }
    }
}