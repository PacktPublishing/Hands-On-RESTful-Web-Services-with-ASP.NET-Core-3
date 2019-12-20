using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.API.Conventions;
using Catalog.API.Filters;
using Catalog.API.ResponseModels;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/items")]
    [ApiController]
    [JsonException]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [ApiConventionMethod(typeof(ItemApiConvention), nameof(ItemApiConvention.Get))]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var result = await _itemService.GetItemsAsync();

            var totalItems = result.Count();

            var itemsOnPage = result
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            var model = new PaginatedItemResponseModel<ItemResponse>(
                pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        [HttpGet("{id:guid}")]
        [ItemExists]
        [ResponseCache(Duration = 100, VaryByQueryKeys = new[] { "*" })]
        [TypeFilter(typeof(RedisCacheFilter), Arguments = new object[] { 20 })]
        [ApiConventionMethod(typeof(ItemApiConvention), nameof(ItemApiConvention.GetById))]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _itemService.GetItemAsync(new GetItemRequest { Id = id });
            return Ok(result);
        }

        [HttpPost]
        [ApiConventionMethod(typeof(ItemApiConvention), nameof(ItemApiConvention.Create))]
        public async Task<IActionResult> Post(AddItemRequest request)
        {
            var result = await _itemService.AddItemAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, null);
        }

        [HttpPut("{id:guid}")]
        [ItemExists]
        [ApiConventionMethod(typeof(ItemApiConvention), nameof(ItemApiConvention.Update))]
        public async Task<IActionResult> Put(Guid id, EditItemRequest request)
        {
            request.Id = id;
            var result = await _itemService.EditItemAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ItemExists]
        [ApiConventionMethod(typeof(ItemApiConvention), nameof(ItemApiConvention.Delete))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteItemRequest { Id = id };

            await _itemService.DeleteItemAsync(request);

            return NoContent();
        }
    }
}