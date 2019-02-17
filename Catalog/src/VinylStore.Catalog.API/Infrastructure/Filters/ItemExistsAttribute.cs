using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VinylStore.Catalog.Domain.Commands.Item;

namespace VinylStore.Catalog.API.Infrastructure.Filters
{
    public class ItemExistsAttribute : TypeFilterAttribute
    {
        public ItemExistsAttribute() : base(typeof
            (ItemExistsFilterImpl))
        {
        }

        public class ItemExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IMediator _mediator;

            public ItemExistsFilterImpl(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context,
                ActionExecutionDelegate next)
            {
                if (!(context.ActionArguments["id"] is Guid id))
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                var result = await _mediator.Send(new GetItemCommand {Id = id});

                if (result == null)
                {
                    context.Result = new NotFoundObjectResult( new JsonErrorPayload {DetailedMessage = $"Item with id {id} not exist."});
                    return;
                }

                await next();
            }
        }
    }
}