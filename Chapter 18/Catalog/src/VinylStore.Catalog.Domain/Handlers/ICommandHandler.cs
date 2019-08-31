using System.Threading;
using System.Threading.Tasks;

namespace VinylStore.Catalog.Domain.Handlers
{
    public interface ICommandHandler<in TCommand, TResponse>
    {
        Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }
}
