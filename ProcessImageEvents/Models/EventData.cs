using Newtonsoft.Json;

namespace ProcessImageEvents.Models
{
    public class EventData
    {
        [JsonProperty("api")]
        public string api { get; set; }

        [JsonProperty("clientRequestId")]
        public string clientRequestId { get; set; }

        [JsonProperty("requestId")]
        public string requestId { get; set; }

        [JsonProperty("eTag")]
        public string eTag { get; set; }

        [JsonProperty("contentType")]
        public string contentType { get; set; }

        [JsonProperty("contentLength")]
        public string contentLength { get; set; }

        [JsonProperty("blobType")]
        public string blobType { get; set; }

        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("sequencer")]
        public string sequencer { get; set; }

        [JsonProperty("StorageDiagnostics")]
        public StorageDiagnostics StorageDiagnostics { get; set; }
    }
}
