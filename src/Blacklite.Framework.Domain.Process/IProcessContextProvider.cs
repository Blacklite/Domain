using System;

namespace Blacklite.Framework.Domain.Process
{
    public interface IProcessContextProvider
    {
        IProcessContext GetContextFor<T>(T instance);
    }
}
