﻿using CryptoExchange.Net.Attributes;

namespace HTX.Net.Enums
{
    /// <summary>
    /// Incident status
    /// </summary>
    public enum IncidentStatus
    {
        /// <summary>
        /// Investigating
        /// </summary>
        [Map("investigating")]
        Investigating,
        /// <summary>
        /// Identified
        /// </summary>
        [Map("identified")]
        Identified,
        /// <summary>
        /// Monitoring
        /// </summary>
        [Map("monitoring")]
        Monitoring,
        /// <summary>
        /// Resolved
        /// </summary>
        [Map("resolved")]
        Resolved
    }
}
