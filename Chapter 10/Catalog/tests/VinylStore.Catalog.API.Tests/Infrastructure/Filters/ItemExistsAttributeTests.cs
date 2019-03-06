using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using VinylStore.Catalog.API.Infrastructure.Filters;
using VinylStore.Catalog.Domain.Commands.Item;
using VinylStore.Catalog.Domain.Responses.Item;
using Xunit;

namespace VinylStore.Catalog.API.Tests.Infrastructure.Filters
{
    public class ItemExistsAttributeTests
    {
        [Fact]
        public async Task should_continue_pipeline_when_id_is_not_present()
        {
            var existingId = Guid.NewGuid();
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(command => command.Send(It.IsAny<GetItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var filter = new ItemExistsAttribute.ItemExistsFilterImpl(mediator.Object);


            var actionExecutedContext = new ActionExecutingContext(
                new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>
                {
                    {"id", existingId}
                }, new object());

            var nextCallback = new Mock<ActionExecutionDelegate>();
            await filter.OnActionExecutionAsync(actionExecutedContext, nextCallback.Object);

            nextCallback.Verify(executionDelegate => executionDelegate(), Times.Never);
        }

        [Fact]
        public async Task should_continue_pipeline_when_id_is_present()
        {
            var id = Guid.NewGuid();

            var mediator = new Mock<IMediator>();
            mediator
                .Setup(_ => _.Send(It.IsAny<GetItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ItemResponse { Id = id });

            var filter = new ItemExistsAttribute.ItemExistsFilterImpl(mediator.Object);


            var actionExecutedContext = new ActionExecutingContext(
                new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>
                {
                    {"id", id}
                }, new object());

            var nextCallback = new Mock<ActionExecutionDelegate>();
            await filter.OnActionExecutionAsync(actionExecutedContext, nextCallback.Object);

            nextCallback.Verify(executionDelegate => executionDelegate(), Times.Once);
        }
    }
}