using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RiskFirst.Hateoas;
using VinylStore.Catalog.API.Infrastructure.Filters;
using VinylStore.Catalog.API.ResponseModels;
using VinylStore.Catalog.Domain.Commands.Item;

namespace VinylStore.Catalog.API.Controllers
{
    [Route("api/hateoas/items")]
    [ApiController]
    [JsonException]
    public class ItemsHateoasController : ControllerBase
    {
        private readonly ILinksService _linksService;
        private readonly IMediator _mediator;

        public ItemsHateoasController(IMediator mediator, ILinksService linkService)
        {
            _mediator = mediator;
            _linksService = linkService;
        }

        [HttpGet(Name = nameof(Get))]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var result = await _mediator.Send(new GetItemsCommand());

            int totalItems = result.Count;

            var itemsOnPage = result
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            var hateoasResults = new List<ItemHateoasResponse>();

            foreach (var itemResponse in itemsOnPage)
            {
                var hateoasResult = new ItemHateoasResponse { Data = itemResponse };
                await _linksService.AddLinksAsync(hateoasResult);

                hateoasResults.Add(hateoasResult);
            }

            var model = new PaginatedItemResponseModel<ItemHateoasResponse>(
                pageIndex, pageSize, totalItems, hateoasResults);

            return Ok(model);
        }


        [HttpGet("{id:guid}", Name = nameof(GetById))]
        [ItemExists]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetItemCommand { Id = id });

            var hateoasResult = new ItemHateoasResponse { Data = result };
            await _linksService.AddLinksAsync(hateoasResult);

            return Ok(hateoasResult);
        }

        [HttpPost(Name = nameof(Post))]
        public async Task<IActionResult> Post(AddItemCommand request)
        {
            var result = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, null);
        }

        [HttpPut("{id:guid}", Name = nameof(Put))]
        [ItemExists]
        public async Task<IActionResult> Put(Guid id, EditItemCommand request)
        {
            request.Id = id;
            var result = await _mediator.Send(request);

            var hateoasResult = new ItemHateoasResponse { Data = result };
            await _linksService.AddLinksAsync(hateoasResult);

            return Ok(hateoasResult);
        }

        [HttpDelete("{id:guid}", Name = nameof(Delete))]
        [ItemExists]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteItemCommand { Id = id };
            await _mediator.Send(request);
            return NoContent();
        }
    }
}
