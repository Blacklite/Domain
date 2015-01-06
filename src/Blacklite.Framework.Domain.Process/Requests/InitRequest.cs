using Blacklite.Framework.Domain.Requests;
using System;

namespace Blacklite.Framework.Domain.Process.Requests
{
        public interface IInitRequest<T, TResponse> : IRequest<TResponse>
        where TResponse : IProcessResponse<T>
        where T : class
        { }

        abstract class InitRequest<T, TResponse> : IInitRequest<T, TResponse>
            where TResponse : IProcessResponse<T>
            where T : class
        { }
}
