using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.API.Filters;
using Catalog.API.ResponseModels;
using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Responses.Item;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/artist")]
    [ApiController]
    [JsonException]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var result = await _artistService.GetArtistsAsync(CancellationToken.None);

            var totalItems = result.ToList().Count;

            var itemsOnPage = result
                .OrderBy(c => c.ArtistName)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            var model = new PaginatedItemResponseModel<ArtistResponse>(
                pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _artistService.GetArtistAsync(new GetArtistRequest { Id = id }, CancellationToken.None);
            return Ok(result);
        }

        [HttpGet("{id:guid}/items")]
        public async Task<IActionResult> GetItemsById(Guid id)
        {
            var result = await _artistService.GetItemByArtistIdAsync(new GetItemsByArtistRequest() { Id = id }, CancellationToken.None);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddArtistRequest request)
        {
            var result = await _artistService.AddArtist(request, CancellationToken.None);
            return CreatedAtAction(nameof(GetById), new { id = result.ArtistId }, null);
        }
    }
}