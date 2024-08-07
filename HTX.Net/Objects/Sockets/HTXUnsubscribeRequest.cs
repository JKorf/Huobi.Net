﻿

namespace HTX.Net.Objects.Sockets
{
    internal class HTXUnsubscribeRequest
    {
        [JsonPropertyName("unsub")]
        public string Topic { get; set; } = string.Empty;
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("data_type"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DataType { get; set; }
    }
}
