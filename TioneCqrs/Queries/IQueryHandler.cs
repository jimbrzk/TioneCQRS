using System.Threading;
using System.Threading.Tasks;

namespace TioneCqrs.Queries
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult> ExecuteAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}