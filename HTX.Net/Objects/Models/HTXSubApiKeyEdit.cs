﻿namespace HTX.Net.Objects.Models
{
    /// <summary>
    /// Edit result
    /// </summary>
    public record HTXSubApiKeyEdit
    {
        /// <summary>
        /// Note
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; } = string.Empty;
        /// <summary>
        /// Permission, comma seperated
        /// </summary>
        [JsonPropertyName("permission")]
        public string Permission { get; set; } = string.Empty;
        /// <summary>
        /// Ip addresses, comma seperated
        /// </summary>
        [JsonPropertyName("ipAddresses")]
        public string IpAddresses { get; set; } = string.Empty;
    }


}
