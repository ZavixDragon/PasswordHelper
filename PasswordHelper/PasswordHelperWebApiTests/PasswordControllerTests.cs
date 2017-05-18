using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PasswordHelper;
using PasswordHelperWebApi;
using PasswordHelperWebApi.PasswordHandling;
using Xunit;

namespace PasswordHelperWebApiTests
{
    public class PasswordControllerTests
    {
        private readonly HttpClient _client;

        public PasswordControllerTests()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GeneratePasswordWithNoIdentifier_BadRequest()
        {
            var request = new GeneratePasswordRequest();

            var response = await _client.PostAsync("/api/password/generate", GetJsonContent(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GeneratePasswordWithOnlyIdentifier_UsesDefaults()
        {
            var response = await _client.PostAsync("/api/password/generate", new StringContent("{ \"Identifier\":\"site\" }", Encoding.UTF8, "application/json"));
            var password = JsonConvert.DeserializeObject<Password>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(password.Value.Length, new GeneratePasswordRequest().Length);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GeneratePasswordCorrectly_ReturnValidPassword()
        {
            var request = new GeneratePasswordRequest { Identifier = "siteName" };

            var response = await _client.PostAsync("/api/password/generate", GetJsonContent(request));
            var password = JsonConvert.DeserializeObject<Password>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(password.Value);
            Assert.NotNull(password.Identifier);
        }


        [Fact]
        [Trait("Category", "Integration")]
        public async Task GeneratePasswordWithLengthSmallerThanInclusions_BadRequest()
        {
            var request = new GeneratePasswordRequest
            {
                Identifier = "",
                Length = 0,
                IncludeNumbers = true
            };

            var response = await _client.PostAsync("/api/password/generate", GetJsonContent(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GeneratePasswordWithNoInclusions_BadRequest()
        {
            var request = new GeneratePasswordRequest
            {
                Identifier = "",
                Length = 1,
                IncludeLowercaseLetters = false,
                IncludeUppercaseLetters = false,
                IncludeNumbers = false,
                IncludeSymbols = false
            };

            var response = await _client.PostAsync("/api/password/generate", GetJsonContent(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task StorePasswordWithNulls_BadRequest()
        {
            var request = new Password(null, null);

            var response = await _client.PostAsync("api/password/save", GetJsonContent(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task StorePassword_OK()
        {
            var request = new Password("id", "password");

            var response = await _client.PostAsync("api/password/save", GetJsonContent(request));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task RetrieveNonExistentPassword_NotFound()
        {
            var response = await _client.GetAsync("api/password/nonexistent");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task RetrievePassword_PasswordRetrieved()
        {
            var request = new Password("id", "password");
            await _client.PostAsync("api/password/save", GetJsonContent(request));

            var response = await _client.GetAsync("api/password/id");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(request.Value, JsonConvert.DeserializeObject<Password>(await response.Content.ReadAsStringAsync()).Value);
        }

        private StringContent GetJsonContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}
