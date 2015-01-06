using System;
using System.Threading.Tasks;

namespace Blacklite.Framework.Domain.Requests
{
    public class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _requestHandler;
        public RequestHandler(IAsyncRequestHandler<TRequest, TResponse> requestHandler)
        {
            _requestHandler = requestHandler;
        }

        public TResponse Handle(TRequest message)
        {
            return Task.Run(() => _requestHandler.Handle(message)).Result;
        }
    }
}
