using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Filters;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace Catalog.API.Tests.Filters
{
    public class ItemExistsAttributeTests
    {
        [Fact]
        public async Task should_continue_pipeline_when_id_is_not_present()
        {
            var existingId = Guid.NewGuid();
            var itemService = new Mock<IItemService>();
            itemService
                .Setup(_ => _.GetItemAsync(It.IsAny<GetItemRequest>()))
                .ReturnsAsync(() => null);

            var filter = new ItemExistsAttribute.ItemExistsFilterImpl(itemService.Object);


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

            var itemService = new Mock<IItemService>();
            itemService
                .Setup(_ => _.GetItemAsync(It.IsAny<GetItemRequest>()))
                .ReturnsAsync(new ItemResponse {Id = id});

            var filter = new ItemExistsAttribute.ItemExistsFilterImpl(itemService.Object);


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