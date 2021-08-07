using System.Text.Json.Serialization;

namespace EncryptionService.Controllers.Models
{
    public class Value
    {
        public Value(string stringValue)
        {
            StringValue = stringValue;
        }

        [JsonPropertyName("value")]
        public string StringValue { get; }
    }
}