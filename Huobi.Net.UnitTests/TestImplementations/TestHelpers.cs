﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using Huobi.Net.Clients.Rest.Spot;
using Huobi.Net.Clients.Socket;
using Huobi.Net.Enums;
using Huobi.Net.Interfaces;
using Huobi.Net.Interfaces.Clients.Rest.Spot;
using Huobi.Net.Objects;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace Huobi.Net.UnitTests.TestImplementations
{
    public class TestHelpers
    {
        [ExcludeFromCodeCoverage]
        public static bool AreEqual<T>(T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                var type = self.GetType();
                var ignoreList = new List<string>(ignore);
                foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (ignoreList.Contains(pi.Name))
                    {
                        continue;
                    }

                    var selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                    var toValue = type.GetProperty(pi.Name).GetValue(to, null);

                    if (pi.PropertyType.IsClass && !pi.PropertyType.Module.ScopeName.Equals("System.Private.CoreLib.dll"))
                    {
                        // Check of "CommonLanguageRuntimeLibrary" is needed because string is also a class
                        if (AreEqual(selfValue, toValue, ignore))
                        {
                            continue;
                        }

                        return false;
                    }

                    if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                    {
                        return false;
                    }
                }

                return true;
            }

            return self == to;
        }

        public static HuobiSocketClientSpot CreateSocketClient(IWebsocket socket, HuobiSocketClientSpotOptions options = null)
        {
            HuobiSocketClientSpot client;
            client = options != null ? new HuobiSocketClientSpot(options) : new HuobiSocketClientSpot(new HuobiSocketClientSpotOptions() { LogLevel = LogLevel.Debug, ApiCredentials = new ApiCredentials("Test", "Test") });
            client.SocketFactory = Mock.Of<IWebsocketFactory>();
            Mock.Get(client.SocketFactory).Setup(f => f.CreateWebsocket(It.IsAny<Log>(), It.IsAny<string>())).Returns(socket);
            return client;
        }

        public static HuobiSocketClientSpot CreateAuthenticatedSocketClient(IWebsocket socket, HuobiSocketClientSpotOptions options = null)
        {
            HuobiSocketClientSpot client;
            client = options != null ? new HuobiSocketClientSpot(options) : new HuobiSocketClientSpot(new HuobiSocketClientSpotOptions(){ LogLevel = LogLevel.Trace, ApiCredentials = new ApiCredentials("Test", "Test")});
            client.SocketFactory = Mock.Of<IWebsocketFactory>();
            Mock.Get(client.SocketFactory).Setup(f => f.CreateWebsocket(It.IsAny<Log>(), It.IsAny<string>())).Returns(socket);  
            return client;
        }

        public static IHuobiClientSpot CreateClient(HuobiClientSpotOptions options = null)
        {
            IHuobiClientSpot client;
            client = options != null ? new HuobiClientSpot(options) : new HuobiClientSpot(new HuobiClientSpotOptions(){LogLevel = LogLevel.Debug});
            client.RequestFactory = Mock.Of<IRequestFactory>();
            return client;
        }

        public static IHuobiClientSpot CreateAuthResponseClient(string response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var client = (HuobiClientSpot)CreateClient(new HuobiClientSpotOptions(){ ApiCredentials = new ApiCredentials("Test", "test")});
            SetResponse(client, response, statusCode);
            return client;
        }


        public static IHuobiClientSpot CreateResponseClient(string response, HuobiClientSpotOptions options = null)
        {
            var client = (HuobiClientSpot)CreateClient(options);
            SetResponse(client, response);
            return client;
        }

        public static IHuobiClientSpot CreateResponseClient<T>(T response, HuobiClientSpotOptions options = null)
        {
            var client = (HuobiClientSpot)CreateClient(options);
            SetResponse(client, JsonConvert.SerializeObject(response));
            return client;
        }

        public static void SetResponse(RestClient client, string responseData, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var expectedBytes = Encoding.UTF8.GetBytes(responseData);
            var responseStream = new MemoryStream();
            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);

            var response = new Mock<IResponse>();
            response.Setup(c => c.IsSuccessStatusCode).Returns(statusCode == HttpStatusCode.OK);
            response.Setup(c => c.StatusCode).Returns(statusCode);
            response.Setup(c => c.GetResponseStreamAsync()).Returns(Task.FromResult((Stream)responseStream));

            var request = new Mock<IRequest>();
            request.Setup(c => c.Uri).Returns(new Uri("http://www.test.com"));
            request.Setup(c => c.GetResponseAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(response.Object));

            var factory = Mock.Get(client.RequestFactory);
            factory.Setup(c => c.Create(It.IsAny<HttpMethod>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(request.Object);
        }

        public static object? GetTestValue(Type type, int i)
        {
            if (type == typeof(bool))
                return true;

            if (type == typeof(bool?))
                return (bool?)true;

            if (type == typeof(decimal))
                return i / 100m;

            if (type == typeof(decimal?))
                return (decimal?)(i / 100m);

            if (type == typeof(int))
                return i + 1;

            if (type == typeof(int?))
                return (int?)i;

            if (type == typeof(long))
                return (long)i;

            if (type == typeof(long?))
                return (long?)i;

            if (type == typeof(DateTime))
                return new DateTime(2019, 1, Math.Max(i, 1));

            if (type == typeof(DateTime?))
                return (DateTime?)new DateTime(2019, 1, Math.Max(i, 1));

            if (type == typeof(string))
                return "STRING" + i;

            if (type == typeof(IEnumerable<string>))
                return new[] { "string" + i };

            if (type == typeof(IEnumerable<OrderState>))
                return new[] { OrderState.Canceled };

            if (type.IsEnum)
            {
                return Activator.CreateInstance(type);
            }

            if (type.IsArray)
            {
                var elementType = type.GetElementType()!;
                var result = Array.CreateInstance(elementType, 2);
                result.SetValue(GetTestValue(elementType, 0), 0);
                result.SetValue(GetTestValue(elementType, 1), 1);
                return result;
            }

            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)))
            {
                var result = (IList)Activator.CreateInstance(type)!;
                result.Add(GetTestValue(type.GetGenericArguments()[0], 0));
                result.Add(GetTestValue(type.GetGenericArguments()[0], 1));
                return result;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var result = (IDictionary)Activator.CreateInstance(type)!;
                result.Add(GetTestValue(type.GetGenericArguments()[0], 0)!, GetTestValue(type.GetGenericArguments()[1], 0));
                result.Add(GetTestValue(type.GetGenericArguments()[0], 1)!, GetTestValue(type.GetGenericArguments()[1], 1));
                return Convert.ChangeType(result, type);
            }

            return null;
        }

        public static async Task<object> InvokeAsync(MethodInfo @this, object obj, params object[] parameters)
        {
            var task = (Task)@this.Invoke(obj, parameters);
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            return resultProperty.GetValue(task);
        }
    }
}
