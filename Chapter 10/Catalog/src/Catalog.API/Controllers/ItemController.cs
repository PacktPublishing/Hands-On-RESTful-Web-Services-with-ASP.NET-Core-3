using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.API.Filters;
using Catalog.API.ResponseModels;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses.Item;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var result = await _itemService.GetItems(CancellationToken.None);

            var totalItems = result.Count;

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
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _itemService.GetItem(new GetItemRequest { Id = id }, CancellationToken.None);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddItemRequest request)
        {
            var result = await _itemService.AddItem(request, CancellationToken.None);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, null);
        }

        [HttpPut("{id:guid}")]
        [ItemExists]
        public async Task<IActionResult> Put(Guid id, EditItemRequest request)
        {
            request.Id = id;
            var result = await _itemService.EditItem(request, CancellationToken.None);
            return Ok(result);
        }
    }
}