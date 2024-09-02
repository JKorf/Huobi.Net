﻿using CryptoExchange.Net.SharedApis.Interfaces;
using CryptoExchange.Net.SharedApis.Interfaces.Rest.Spot;
using System;
using System.Collections.Generic;
using System.Text;

namespace HTX.Net.Interfaces.Clients.SpotApi
{
    public interface IHTXRestClientSpotApiShared :
        IAssetsRestClient,
        IBalanceRestClient,
        IDepositRestClient,
        IKlineRestClient,
        IOrderBookRestClient,
        IRecentTradeRestClient,
        ISpotOrderRestClient,
        ISpotSymbolRestClient,
        ITickerRestClient,
        //ITradeHistoryRestClient
        IWithdrawalRestClient,
        IWithdrawRestClient
    {
    }
}
