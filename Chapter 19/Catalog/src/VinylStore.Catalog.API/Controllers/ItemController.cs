using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VinylStore.Catalog.API.Infrastructure.Conventions.VinylStore.Catalog.API.Infrastructure.Conventions;
using VinylStore.Catalog.API.Infrastructure.Filters;
using VinylStore.Catalog.API.ResponseModels;
using VinylStore.Catalog.Domain.Commands.Item;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.API.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ApiConventionMethod(typeof(ItemApiConvention), nameof(ItemApiConvention.Get))]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var result = await _mediator.Send(new GetItemsCommand());

            int totalItems = result.Count;

            var itemsOnPage = result
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            var model = new PaginatedItemResponseModel<ItemResponse>(
                pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        [HttpGet("{id:guid}")]
        [ResponseCache(Duration = 20)]
        [ItemExists]
        [ApiConventionMethod(typeof(ItemApiConvention), nameof(ItemApiConvention.GetById))]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetItemCommand { Id = id });
            return Ok(result);
        }

        [HttpPost]
        [ApiConventionMethod(typeof(ItemApiConvention), nameof(ItemApiConvention.Create))]
        public async Task<IActionResult> Post(AddItemCommand request)
        {
            var result = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, null);
        }

        [HttpPut("{id:guid}")]
        [ItemExists]
        [ApiConventionMethod(typeof(ItemApiConvention), nameof(ItemApiConvention.Update))]
        public async Task<IActionResult> Put(Guid id, EditItemCommand request)
        {
            request.Id = id;
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ItemExists]
        [ApiConventionMethod(typeof(ItemApiConvention), nameof(ItemApiConvention.Delete))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteItemCommand { Id = id };
            await _mediator.Send(request);
            return NoContent();
        }
    }
}
