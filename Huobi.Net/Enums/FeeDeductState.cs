﻿using CryptoExchange.Net.Attributes;

namespace HTX.Net.Enums
{
    /// <summary>
    /// Fee deduction status.
    /// </summary>
    public enum FeeDeductState
    {
        /// <summary>
        /// In deduction
        /// </summary>
        [Map("ongoing")]
        Ongoing,
        /// <summary>
        /// Deduction completed
        /// </summary>
        [Map("done")]
        Done,
    }
}
