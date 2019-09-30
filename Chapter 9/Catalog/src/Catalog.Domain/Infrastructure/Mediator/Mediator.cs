using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Domain.Infrastructure.Mediator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();

            return await handler.Handle(command, cancellationToken);
        }
    }
}