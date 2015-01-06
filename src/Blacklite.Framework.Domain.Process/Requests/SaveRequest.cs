using Blacklite.Framework.Domain.Requests;
using System;

namespace Blacklite.Framework.Domain.Process.Requests
{
    public interface ISaveRequest<T> : IRequest<IProcessResponse<T>>
        where T : class
    {
        T Request { get; }
    }

    public class SaveRequest<T> : ISaveRequest<T>
        where T : class
    {
        public SaveRequest(T instance)
        {
            Request = instance;
        }

        public T Request { get; }
    }
}
