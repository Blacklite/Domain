using System;

namespace Blacklite.Framework.Domain.Requests
{
    public interface IRequest { }
    public interface IRequest<out TResponse> : IRequest { }
}
