using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using VinylStore.Catalog.API.ResponseModels;
using VinylStore.Catalog.Domain.Entities;
using VinylStore.Catalog.Domain.Responses.Item;
using VinylStore.Catalog.Fixtures;
using Xunit;

namespace VinylStore.Catalog.API.Tests.Controllers
{
    public class GenreControllerTests : IClassFixture<InMemoryApplicationFactory<Startup>>
    {
        public GenreControllerTests(InMemoryApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly InMemoryApplicationFactory<Startup> _factory;

        [Theory]
        [InlineData("/api/genre/")]
        public async Task smoke_test_on_items(string url)

        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/api/genre/?pageSize=1&pageIndex=0", 1, 0)]
        public async Task get_should_returns_paginated_data(string url, int pageSize, int pageIndex)

        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity =
                JsonConvert.DeserializeObject<PaginatedItemResponseModel<GenreResponse>>(responseContent);

            responseEntity.PageIndex.ShouldBe(pageIndex);
            responseEntity.PageSize.ShouldBe(pageSize);
            responseEntity.Data.Count().ShouldBe(pageSize);
        }

        [Theory]
        [LoadTestData("record-test.json", "genre_with_id")]
        public async Task get_by_id_should_return_right_data(object jsonPayload)
        {
            var request = JsonConvert.DeserializeObject<Genre>(jsonPayload.ToString());

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/api/genre/{request.GenreId}");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<Genre>(responseContent);

            responseEntity.GenreId.ShouldBe(request.GenreId);
            responseEntity.GenreDescription.ShouldBe(request.GenreDescription);
        }


        [Theory]
        [LoadTestData("record-test.json", "genre_with_id")]
        public async Task get_item_by_genre_should_return_right_data(object jsonPayload)
        {
            var request = JsonConvert.DeserializeObject<Genre>(jsonPayload.ToString());

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/api/genre/{request.GenreId}/items");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<List<ItemResponse>>(responseContent);

            responseEntity.Count.ShouldBe(4);
        }

        [Fact]
        public async Task add_should_create_new_genre()
        {
            var genreDescription = new { GenreDescription = "Jazz" };
            var client = _factory.CreateClient();

            var httpContent = new StringContent(JsonConvert.SerializeObject(genreDescription), Encoding.UTF8,
                "application/json");
            var response = await client.PostAsync("/api/genre", httpContent);

            response.EnsureSuccessStatusCode();

            var responseHeader = response.Headers.Location;

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            responseHeader.ToString().ShouldContain("/api/genre/");
        }
    }
}