using Microsoft.AspNet.Http;
using System;
using System.Reflection;

namespace Blacklite.Framework.Domain.Process
{
    public interface IStep
    {
        StepPhase Phase { get; }
    }

    public interface ICanExecuteStep
    {
        bool CanExecute([NotNull] object instance, HttpContext context);
    }

    public interface ICanRunStep
    {
        bool CanRun([NotNull] Type type);
    }

    public abstract class ProcessStep<T> : IStep, ICanExecuteStep, ICanRunStep
        where T : class
    {
        public abstract StepPhase Phase { get; }

        public virtual bool CanExecute(T instance, [NotNull] HttpContext httpContext) => true;

        public virtual bool CanRun(Type type) => typeof(T).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());

        bool ICanExecuteStep.CanExecute(object context, [NotNull] HttpContext httpContext) => CanExecute((T)context, httpContext);
    }
}
