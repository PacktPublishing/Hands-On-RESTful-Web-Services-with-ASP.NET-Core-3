using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalog.API.Filters
{
    public class ItemExistsAttribute : TypeFilterAttribute
    {
        public ItemExistsAttribute() : base(typeof
            (ItemExistsFilterImpl))
        {
        }

        public class ItemExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IItemService _itemService;

            public ItemExistsFilterImpl(IItemService itemService)
            {
                _itemService = itemService;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context,
                ActionExecutionDelegate next)
            {
                if (!(context.ActionArguments["id"] is Guid id))
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                var result = await _itemService.GetItemAsync(new GetItemRequest { Id = id });

                if (result == null)
                {
                    context.Result = new NotFoundObjectResult(new JsonErrorPayload { DetailedMessage = $"Item with id {id} not exist." });
                    return;
                }

                await next();
            }
        }
    }
}