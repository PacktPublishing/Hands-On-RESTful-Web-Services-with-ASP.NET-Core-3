using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.API.Filters;
using Catalog.API.ResponseModels;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using RiskFirst.Hateoas;

namespace Catalog.API.Controllers
{
    [Route("api/hateoas/items")]
    [ApiController]
    [JsonException]
    public class ItemsHateoasController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ILinksService _linksService;

        public ItemsHateoasController(ILinksService linkService, IItemService itemService)
        {
            _linksService = linkService;
            _itemService = itemService;
        }

        [HttpGet(Name = nameof(Get))]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var result = await _itemService.GetItemsAsync();

            var totalItems = result.Count();

            var itemsOnPage = result.OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            var hateoasResults = new List<ItemHateoasResponse>();

            foreach (var itemResponse in itemsOnPage)
            {
                var hateoasResult = new ItemHateoasResponse {Data = itemResponse};
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
            var result = await _itemService.GetItemAsync(new GetItemRequest {Id = id});
            var hateoasResult = new ItemHateoasResponse {Data = result};
            await _linksService.AddLinksAsync(hateoasResult);

            return Ok(hateoasResult);
        }

        [HttpPost(Name = nameof(Post))]
        public async Task<IActionResult> Post(AddItemRequest request)
        {
            var result = await _itemService.AddItemAsync(request, CancellationToken.None);
            return CreatedAtAction(nameof(GetById), new {id = result.Id}, null);
        }

        [HttpPut("{id:guid}", Name = nameof(Put))]
        [ItemExists]
        public async Task<IActionResult> Put(Guid id, EditItemRequest request)
        {
            request.Id = id;
            var result = await _itemService.EditItemAsync(request, CancellationToken.None);

            var hateoasResult = new ItemHateoasResponse {Data = result};
            await _linksService.AddLinksAsync(hateoasResult);

            return Ok(hateoasResult);
        }

        [HttpDelete("{id:guid}", Name = nameof(Delete))]
        [ItemExists]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteItemRequest {Id = id};
            await _itemService.DeleteItemAsync(request);
            return NoContent();
        }
    }
}