using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Domain.Mediator
{
    public interface IMediator
    {
        Task<TResponse> Send<TCommand, TResponse>(TCommand command,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}