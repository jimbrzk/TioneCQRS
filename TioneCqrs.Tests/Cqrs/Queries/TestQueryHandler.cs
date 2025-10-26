using System.Threading;
using System.Threading.Tasks;
using TioneCqrs.Queries;

namespace TioneCqrs.Tests.Cqrs.Queries
{
    public class TestQueryHandler : IQueryHandler<TestQuery, string>
    {
        public Task<string> ExecuteAsync(TestQuery query, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(query.Test);
        }
    }
}