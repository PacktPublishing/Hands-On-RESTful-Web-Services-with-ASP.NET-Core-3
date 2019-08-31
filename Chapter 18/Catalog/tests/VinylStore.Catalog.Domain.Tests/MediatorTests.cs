using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VinylStore.Catalog.Domain.Commands;
using VinylStore.Catalog.Domain.Handlers;
using VinylStore.Catalog.Domain.Infrastructure.Mediator;
using Xunit;

namespace VinylStore.Catalog.Domain.Tests
{
    public class MediatorTests
    {
        [Fact]
        public void send_should_execute_the_right_handle_method()
        {
            var handler = new Mock<ICommandHandler<FakeCommand, FakeResponse>>();

            var serviceProvider = new ServiceCollection()
                .AddScoped(x => handler.Object)
                .BuildServiceProvider();
            IMediator sut = new Mediator(serviceProvider);

            sut.Send<FakeCommand, FakeResponse>(new FakeCommand());
            handler.Verify(x => x.Handle(It.IsAny<FakeCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    public class FakeResponse
    {
    }

    public class FakeCommand : ICommand
    {
    }
}
