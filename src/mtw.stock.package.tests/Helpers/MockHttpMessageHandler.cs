using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace mtw.stock.package.tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public bool ThrowTimeoutException { get; set; }
        public bool ThrowUnexpectedException { get; set; }

        private Dictionary<string, HttpResponseMessage> _responses;
        private HttpResponseMessage _messageToReturn;
        private string _expectedRequestPath;

        public MockHttpMessageHandler(HttpResponseMessage messageToReturn, string expectedRequestPath)
            : this(new Dictionary<string, HttpResponseMessage> { { expectedRequestPath, messageToReturn } })
        {}

        public MockHttpMessageHandler(Dictionary<string, HttpResponseMessage> responses)
        {
            _responses = responses;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (ThrowTimeoutException)
            {
                throw new TimeoutException();
            }

            if (ThrowUnexpectedException)
            {
                throw new InsufficientMemoryException();
            }

            if (_responses.ContainsKey(request.RequestUri.PathAndQuery))
            {
                return _responses[request.RequestUri.PathAndQuery];
            }
            else
            {
                return null;
            }
        }
    }
}
