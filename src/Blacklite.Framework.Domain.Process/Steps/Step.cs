using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blacklite.Framework.Domain.Process.Steps
{
    /// <summary>
    /// Defines a "Step" of the process, that is a small isolated unit of work.
    ///
    /// Every step must implement this interface plus two methods:
    ///
    /// bool CanExecute(object|T instance[, IProcessContext context]);
    ///
    /// Basically the CanExecute method, can either take a "object", or it can be specificly
    ///     implemented to take a specific type like the Step generic class does.
    ///
    /// void|IEnumerable[IValidation] Execute(object|T instance[, IProcessContext context[, ... Any interface that IServiceProvider can resolve]]);
    ///
    /// The execute method is "Injectable" this means that you can add any value you want onto it that you can possibly imagine.  These values are all serviced through the service provider interface.
    /// In addition there are two special params, the object instance, and IProcessContext, which gets the process context used for this request.
    ///     Both of these special params can be more specificly typed as appropriate, but if the type is wrong you'll run into errors are runtime.
    /// </summary>
    public interface IStep
    {
        IEnumerable<IStepPhase> Phases { get; }
        bool CanRun([NotNull] Type type);
    }

    /// <summary>
    /// A predefined method for defining the "CanExecute" method of a step.
    /// Every step must have a method that returns a bool called "CanExecute",
    ///     but it is it not explictly specified on the interface.
    /// </summary>
    public interface ICanExecuteStep
    {
        bool CanExecute([NotNull] object instance, [NotNull] IProcessContext context);
    }

    /// <summary>
    /// A generic step interface that implements a Typed version of CanExecute, and filters the step based on the Type of T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Step<T> : IStep, ICanExecuteStep
        where T : class
    {
        public abstract IEnumerable<IStepPhase> Phases { get; }

        public virtual bool CanExecute(T instance, IProcessContext context) => true;

        public virtual bool CanRun(Type type) => typeof(T).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());

        bool ICanExecuteStep.CanExecute(object instance, IProcessContext context) => CanExecute((T)instance, context);
    }
}
