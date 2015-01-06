using Blacklite.Framework.Domain.Process;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Process.Tests.Fixtures
{
    [BeforeStep(typeof(StepInitPhases))]
    public class StepInit : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(object context)
        {
        }
    }

    [BeforeStep(typeof(StepAllPhases))]
    public class StepPostInit : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.PostInit;

        public void Execute(HttpContext httpContext, object context)
        {
        }
    }

    [AfterStep(typeof(StepAllPhases))]
    public class StepPreInit : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.PreInit;

        public IEnumerable<IValidation> Execute(object context)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    public class StepInitPhases : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.InitPhases;

        public IEnumerable<IValidation> Execute(HttpContext httpContext, object context)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    [BeforeStep(typeof(StepAllPhases))]
    public class StepPreSave : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.PreSave;

        public IEnumerable<IValidation> Execute(object context, HttpContext httpContext)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    public interface IInjectable { }

    [BeforeStep(typeof(StepAllPhases))]
    public class StepSave : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Save;

        public void Execute(object context, HttpContext httpContext, IInjectable injectable)
        {
        }
    }

    [BeforeStep(typeof(StepAllPhases))]
    public class StepValidate : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Validate;

        public IEnumerable<IValidation> Execute(object context, IInjectable injectable)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    [BeforeStep(typeof(StepSavePhases))]
    [BeforeStep(typeof(StepAllPhases))]
    public class StepPostSave : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.PostSave;

        public void Execute(object context, IInjectable injectable)
        {
        }
    }
    public class StepSavePhases : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.SavePhases;

        public IEnumerable<IValidation> Execute(object context, HttpContext httpContext, IInjectable injectable)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    [AfterStep(typeof(StepInitPhases))]
    [AfterStep(typeof(StepSavePhases))]
    public class StepAllPhases : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.AllPhases;

        public IEnumerable<IValidation> Execute(object context, HttpContext httpContext, IInjectable injectable)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    public abstract class StepVoidExecute : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public abstract void Execute(object context);
    }

    public abstract class StepVoidExecuteContext : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public abstract void Execute(object context, HttpContext httpContext);
    }

    public abstract class StepVoidExecuteInjectable : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public abstract void Execute(object context, HttpContext httpContext, IInjectable injectable);
    }

    public abstract class StepValidationExecute : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public abstract IEnumerable<IValidation> Execute(object context);
    }

    public abstract class StepValidationExecuteContext : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public abstract IEnumerable<IValidation> Execute(object context, HttpContext httpContext);
    }

    public abstract class StepValidationExecuteInjectable : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public abstract IEnumerable<IValidation> Execute(object context, HttpContext httpContext, IInjectable injectable);
    }

    public abstract class CustomStepCanExecute : IStep
    {
        public virtual StepPhase Phase { get; } = StepPhase.Init;

        public abstract bool CanExecute(object context);

        public abstract bool CanRun(Type type);

        public abstract IEnumerable<IValidation> Execute(object context);
    }

    public abstract class CustomStepCanExecuteContext : IStep
    {
        public virtual StepPhase Phase { get; } = StepPhase.Init;

        public abstract bool CanExecute(HttpContext httpContext, object context);

        public abstract bool CanRun(Type type);

        public abstract IEnumerable<IValidation> Execute(object context);
    }

    public abstract class CustomStepCanExecuteContext2 : IStep
    {
        public virtual StepPhase Phase { get; } = StepPhase.Init;

        public abstract bool CanExecute(object context, HttpContext httpContext);

        public abstract bool CanRun(Type type);

        public abstract IEnumerable<IValidation> Execute(object context);
    }

    public abstract class CustomStepCanExecuteInvalid : IStep
    {
        public virtual StepPhase Phase { get; } = StepPhase.Init;

        public abstract bool CanExecute(object context, HttpContext httpContext, IInjectable injectable);

        public abstract bool CanRun(Type type);

        public abstract IEnumerable<IValidation> Execute(object context);
    }

    [BeforeStep(typeof(CyclicBefore1StepB))]
    public class CyclicBefore1StepA : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(object context)
        {
        }
    }

    [BeforeStep(typeof(CyclicBefore1StepA))]
    public class CyclicBefore1StepB : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(HttpContext httpContext, object context)
        {
        }
    }

    [BeforeStep(typeof(CyclicBefore2StepB))]
    public class CyclicBefore2StepA : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(HttpContext httpContext, object context)
        {
        }
    }

    [BeforeStep(typeof(CyclicBefore2StepC))]
    public class CyclicBefore2StepB : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(HttpContext httpContext, object context)
        {
        }
    }

    [BeforeStep(typeof(CyclicBefore2StepA))]
    public class CyclicBefore2StepC : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(HttpContext httpContext, object context)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter1StepB))]
    public class CyclicAfter1StepA : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(object context)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter1StepA))]
    public class CyclicAfter1StepB : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(HttpContext httpContext, object context)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter2StepB))]
    public class CyclicAfter2StepA : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(HttpContext httpContext, object context)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter2StepC))]
    public class CyclicAfter2StepB : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(HttpContext httpContext, object context)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter2StepA))]
    public class CyclicAfter2StepC : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(HttpContext httpContext, object context)
        {
        }
    }

    [BeforeStep(typeof(CyclicBeforeAfterStep1StepB))]
    [AfterStep(typeof(CyclicBeforeAfterStep1StepC))]
    public class CyclicBeforeAfterStep1StepA : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(HttpContext httpContext, object context)
        {
        }
    }
    public class CyclicBeforeAfterStep1StepB : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(HttpContext httpContext, object context)
        {
        }
    }

    [AfterStep(typeof(CyclicBeforeAfterStep1StepB))]
    public class CyclicBeforeAfterStep1StepC : ProcessStep<object>
    {
        public override StepPhase Phase { get; } = StepPhase.Init;

        public void Execute(HttpContext httpContext, object context)
        {
        }
    }
}
