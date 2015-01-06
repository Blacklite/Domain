using System;

namespace Blacklite.Framework.Domain.Requests
{
    public interface IRequestHandler<in TRequest, out TResponse>
        where TRequest : IRequest<TResponse>
    {
        TResponse Handle(TRequest message);
    }

    public interface IPreRequestHandler<in TRequest>
        where TRequest : IRequest
    {
        void Handle(TRequest request);
    }

    public interface IPostRequestHandler<in TRequest, in TResponse>
        where TRequest : IRequest<TResponse>
    {
        void Handle(TRequest request, TResponse response);
    }
}
