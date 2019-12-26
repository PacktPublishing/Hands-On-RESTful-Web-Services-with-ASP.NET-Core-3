using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.API.Filters;
using Catalog.API.ResponseModels;
using Catalog.Domain.Requests.Genre;
using Catalog.Domain.Responses;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/genre")]
    [ApiController]
    [JsonException]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var result = await _genreService.GetGenreAsync();

            var totalItems = result.ToList().Count;

            var itemsOnPage = result
                .OrderBy(c => c.GenreDescription)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            var model = new PaginatedItemResponseModel<GenreResponse>(
                pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _genreService.GetGenreAsync(new GetGenreRequest { Id = id });
            return Ok(result);
        }

        [HttpGet("{id:guid}/items")]
        public async Task<IActionResult> GetItemById(Guid id)
        {
            var result = await _genreService.GetItemByGenreIdAsync(new GetGenreRequest { Id = id });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddGenreRequest request)
        {
            var result = await _genreService.AddGenreAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.GenreId }, null);
        }
    }
}