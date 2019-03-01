using System.Threading;
using System.Threading.Tasks;

namespace VinylStore.Catalog.Domain.Infrastructure.Mediator
{
    public interface IMediator
    {
        Task<TResponse> Send<TCommand, TResponse>(TCommand command,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}