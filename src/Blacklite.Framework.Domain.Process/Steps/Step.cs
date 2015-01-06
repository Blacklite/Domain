using System;
using System.Reflection;

namespace Blacklite.Framework.Domain.Process.Steps
{
    public interface IStep
    {
        StepPhase Phase { get; }
        bool CanRun([NotNull] Type type);
    }

    public interface ICanExecuteStep
    {
        bool CanExecute([NotNull] object instance, [NotNull] IProcessContext context);
    }

    public abstract class Step<T> : IStep, ICanExecuteStep
        where T : class
    {
        public abstract StepPhase Phase { get; }

        public virtual bool CanExecute(T instance, IProcessContext context) => true;

        public virtual bool CanRun(Type type) => typeof(T).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());

        bool ICanExecuteStep.CanExecute(object instance, IProcessContext context) => CanExecute((T)instance, context);
    }
}
