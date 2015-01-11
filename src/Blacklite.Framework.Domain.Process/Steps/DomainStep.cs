using Blacklite.Framework.Steps;
using System;

namespace Blacklite.Framework.Domain.Process.Steps
{
    public interface IDomainStep : IPhasedStep
    {

    }

    public abstract class DomainStep<T> : PhasedStep<T>, IDomainStep where T : class { }
}
