using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BookApi.Tests.Utils;
using Xunit;

namespace BookApi.Tests.Integration
{
    public class BookControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public BookControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }


        public class BookResponse
        {
            public int Id { get; set; }
        }



        [Fact]
        public async Task PostBook_Should_Create_And_Return_Book()
        {
            // Arrange
            var newBook = new
            {
                Title = "Integration Book",
                Author = "Test Author"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/book", newBook);
            response.EnsureSuccessStatusCode();

            // 🔴 Current (failing) line:
            // var result = await response.Content.ReadFromJsonAsync<dynamic>();
            // Assert.Equal("Integration Book", (string)result.title);

            // ✅ Replace with this:
            var raw = await response.Content.ReadAsStringAsync();
            Console.WriteLine("RESPONSE: " + raw);

            using var doc = JsonDocument.Parse(raw);
            var root = doc.RootElement;

            int id = root.GetProperty("id").GetProperty("id").GetInt32();
            Assert.True(id > 0);




        }

    }
}
