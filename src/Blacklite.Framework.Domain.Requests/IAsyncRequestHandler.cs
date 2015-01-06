using System;
using System.Threading.Tasks;

namespace Blacklite.Framework.Domain.Requests
{
    public interface IAsyncRequestHandler<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
        Task<TResponse> Handle(TRequest message);
    }
}
