﻿using HTX.Net.Interfaces.Clients.SpotApi;
using HTX.Net.Interfaces.Clients.UsdtFuturesApi;

namespace HTX.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the HTX websocket API. 
    /// </summary>
    public interface IHTXSocketClient : ISocketClient
    {
        /// <summary>
        /// Spot streams
        /// </summary>
        public IHTXSocketClientSpotApi SpotApi { get; }
        /// <summary>
        /// Usdt futures streams
        /// </summary>
        public IHTXSocketClientUsdtFuturesApi UsdtFuturesApi { get; }

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}