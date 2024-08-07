﻿using CryptoExchange.Net.Attributes;

namespace HTX.Net.Enums
{
    /// <summary>
    /// Margin mode type
    /// </summary>
    public enum ElementModeType
    {
        /// <summary>
        /// Isolated margin
        /// </summary>
        [Map("1")]
        IsolatedMargin,
        /// <summary>
        /// Cross and isolated margin
        /// </summary>
        [Map("2")]
        CrossAndIsolatedMargin,
        /// <summary>
        /// Cross margin
        /// </summary>
        [Map("3")]
        CrossMargin
    }
}
