using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EncryptionService.Test.Integration.API
{
    public class EncryptionIntegrationTests : IntegrationTestBase
    {
        private class ValueModel
        {
            public ValueModel(string value)
            {
                Value = value;
            }

            public string Value { get; }
        }
        
        public EncryptionIntegrationTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        private async Task<HttpResponseMessage> EncryptValue(string value)
        {
            var client = GetHttpClient();
            var dataToEncrypt = new ValueModel(value);
            return await client.PostAsync("api/v1/encrypt", CreateHttpContent(dataToEncrypt));
        }
        
        private async Task<HttpResponseMessage> DecryptValue(string value)
        {
            var client = GetHttpClient();
            var dataToDecrypt = new ValueModel(value);
            return await client.PostAsync("api/v1/decrypt", CreateHttpContent(dataToDecrypt));
        }

        [Fact]
        public async void Can_Encrypt_And_Decrypt_Value()
        {
            var value = "somevalue";

            var encryptionResponse = await EncryptValue(value);
            encryptionResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            var encryptionResponseContent = await DeserializeContent<ValueModel>(encryptionResponse.Content);

            var decryptionResponse = await DecryptValue(encryptionResponseContent.Value);
            decryptionResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            var decryptionResponseContent = await DeserializeContent<ValueModel>(decryptionResponse.Content);

            decryptionResponseContent.Value.Should().Be(value);
        }
        
        [Fact]
        public async void Decrypting_Invalid_Value_Returns_400()
        {
            var decryptionResponse = await DecryptValue("invalidformat");
            decryptionResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}