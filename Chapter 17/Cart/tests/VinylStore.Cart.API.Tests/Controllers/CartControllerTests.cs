using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using VinylStore.Cart.Domain.Responses.Cart;
using VinylStore.Cart.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace VinylStore.Cart.API.Tests.Controllers
{
    public class CartControllerTests : IClassFixture<CartApplicationFactory<Startup>>
    {
        private readonly CartApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;

        public CartControllerTests(CartApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("/api/cart/9ced6bfa-9659-462e-aece-49fe50613e96")]
        public async Task smoke_test_on_get(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/api/cart/9ced6bfa-9659-462e-aece-49fe50613e96")]
        public async Task get_should_return_right_cart(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<CartExtendedResponse>(responseContent);

            responseData.Id.ShouldBe("9ced6bfa-9659-462e-aece-49fe50613e96");
            responseData.Items.Count.ShouldNotBeNull();
            responseData.User.Email.ShouldNotBeEmpty();
        }

        [Theory]
        [LoadTestData("record-data.json", "item_without_id")]
        public async Task post_should_create_a_cart(object jsonPayload)
        {
            var client = _factory.CreateClient();

            var httpContent = new StringContent(jsonPayload.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/cart", httpContent);

            response.EnsureSuccessStatusCode();

            var responseHeader = response.Headers.Location;
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            responseHeader.ToString().Contains("/api/cart").ShouldBeTrue();
        }

        [Theory]
        [InlineData("9ced6bfa-9659-462e-aece-49fe50613e96", "f5da5ce4-091e-492e-a70a-22b073d75a52")]
        public async Task put_should_should_increase_cart_quantity(string cartId, string cartItemId)
        {
            var client = _factory.CreateClient();

            var response = await client.PutAsync($"/api/cart/{cartId}/items/{cartItemId}",
                new StringContent(string.Empty));

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("9ced6bfa-9659-462e-aece-49fe50613e96", "f5da5ce4-091e-492e-a70a-22b073d75a52")]
        public async Task delete_should_should_decrease_cart_quantity(string cartId, string cartItemId)
        {
            var client = _factory.CreateClient();

            var response = await client.PutAsync($"/api/cart/{cartId}/items/{cartItemId}",
                new StringContent(string.Empty));

            string test = await response.Content.ReadAsStringAsync();
            Console.WriteLine(test);
            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
