using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EncryptionService.Test.Integration
{
    public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        protected IntegrationTestBase(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        protected HttpClient GetHttpClient()
        {
            var client = _factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });

            return client;
        }

        protected HttpContent CreateHttpContent<T>(T obj)
        {
            var dataAsString = JsonSerializer.Serialize(
                obj,
                new JsonSerializerOptions
                {
                    IgnoreNullValues = true
                }
            );
            var stringContent = new StringContent(dataAsString);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return stringContent;
        }
        
        protected async Task<T> DeserializeContent<T>(HttpContent httpContent)
        {
            var body = await httpContent.ReadAsStringAsync();
            
            return JsonSerializer.Deserialize<T>(body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}