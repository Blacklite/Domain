using System;

namespace Blacklite.Framework.Domain.Process
{
    public interface IProcessContext
    {
        IServiceProvider ProcessServices { get; }
    }
}
