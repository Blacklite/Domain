using System;
using System.Threading.Tasks;

namespace Blacklite.Framework.Domain.Requests
{
    public interface IAsyncRequestHandler<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest message);
    }

    public interface IAsyncPreRequestHandler<in TRequest>
        where TRequest : IRequest
    {
        Task Handle(TRequest request);
    }

    public interface IAsyncPostRequestHandler<in TRequest, in TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task Handle(TRequest request, TResponse response);
    }
}
