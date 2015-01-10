using Blacklite.Framework.Domain.Process;
using Blacklite.Framework.Domain.Process.Steps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Process.Tests.Fixtures
{
    [BeforeStep(typeof(StepInitPhases))]
    public class StepInit : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(object instance)
        {
        }
    }

    [BeforeStep(typeof(StepAllPhases))]
    public class StepPostInit : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.PostInit;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [AfterStep(typeof(StepAllPhases))]
    public class StepPreInit : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.PreInit;

        public IEnumerable<IValidation> Execute(object instance)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    public class StepInitPhases : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.InitPhases;

        public IEnumerable<IValidation> Execute(IProcessContext context, object instance)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    [BeforeStep(typeof(StepAllPhases))]
    public class StepPreSave : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.PreSave;
        public IEnumerable<IValidation> Execute(object instance, IProcessContext context)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    public interface IInjectable { }

    [BeforeStep(typeof(StepAllPhases))]
    public class StepSave : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Save;
        public void Execute(object instance, IProcessContext context, IInjectable injectable)
        {
        }
    }

    [BeforeStep(typeof(StepAllPhases))]
    public class StepValidate : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Validate;
        public IEnumerable<IValidation> Execute(object instance, IInjectable injectable)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    [BeforeStep(typeof(StepSavePhases))]
    [BeforeStep(typeof(StepAllPhases))]
    public class StepPostSave : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.PostSave;
        public void Execute(object instance, IInjectable injectable)
        {
        }
    }
    public class StepSavePhases : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.SavePhases;
        public IEnumerable<IValidation> Execute(object instance, IProcessContext context, IInjectable injectable)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    [AfterStep(typeof(StepInitPhases))]
    [AfterStep(typeof(StepSavePhases))]
    public class StepAllPhases : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.AllPhases;
        public IEnumerable<IValidation> Execute(object instance, IProcessContext context, IInjectable injectable)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    public abstract class StepVoidExecute : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public abstract void Execute(object instance);
    }

    public abstract class StepVoidExecuteContext : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public abstract void Execute(object instance, IProcessContext context);
    }

    public abstract class StepVoidExecuteInjectable : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public abstract void Execute(object instance, IProcessContext context, IInjectable injectable);
    }

    public abstract class StepValidationExecute : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public abstract IEnumerable<IValidation> Execute(object instance);
    }

    public abstract class StepValidationExecuteContext : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public abstract IEnumerable<IValidation> Execute(object instance, IProcessContext context);
    }

    public abstract class StepValidationExecuteInjectable : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public abstract IEnumerable<IValidation> Execute(object instance, IProcessContext context, IInjectable injectable);
    }

    public abstract class CustomStepCanExecute : IStep
    {
        public virtual IEnumerable<IStepPhase> Phases { get; } = StepPhase.Init;

        public abstract bool CanExecute(object instance);

        public abstract bool CanRun(Type type);

        public abstract IEnumerable<IValidation> Execute(object instance);
    }

    public abstract class CustomStepCanExecuteContext : IStep
    {
        public virtual IEnumerable<IStepPhase> Phases { get; } = StepPhase.Init;

        public abstract bool CanExecute(IProcessContext context, object instance);

        public abstract bool CanRun(Type type);

        public abstract IEnumerable<IValidation> Execute(object instance);
    }

    public abstract class CustomStepCanExecuteContext2 : IStep
    {
        public virtual IEnumerable<IStepPhase> Phases { get; } = StepPhase.Init;

        public abstract bool CanExecute(object instance, IProcessContext context);

        public abstract bool CanRun(Type type);

        public abstract IEnumerable<IValidation> Execute(object instance);
    }

    public abstract class CustomStepCanExecuteInvalid : IStep
    {
        public virtual IEnumerable<IStepPhase> Phases { get; } = StepPhase.Init;

        public abstract bool CanExecute(object instance, IProcessContext context, IInjectable injectable);

        public abstract bool CanRun(Type type);

        public abstract IEnumerable<IValidation> Execute(object instance);
    }

    [BeforeStep(typeof(CyclicBefore1StepB))]
    public class CyclicBefore1StepA : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(object instance)
        {
        }
    }

    [BeforeStep(typeof(CyclicBefore1StepA))]
    public class CyclicBefore1StepB : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [BeforeStep(typeof(CyclicBefore2StepB))]
    public class CyclicBefore2StepA : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [BeforeStep(typeof(CyclicBefore2StepC))]
    public class CyclicBefore2StepB : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [BeforeStep(typeof(CyclicBefore2StepA))]
    public class CyclicBefore2StepC : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter1StepB))]
    public class CyclicAfter1StepA : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(object instance)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter1StepA))]
    public class CyclicAfter1StepB : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter2StepB))]
    public class CyclicAfter2StepA : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter2StepC))]
    public class CyclicAfter2StepB : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter2StepA))]
    public class CyclicAfter2StepC : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [BeforeStep(typeof(CyclicBeforeAfterStep1StepB))]
    [AfterStep(typeof(CyclicBeforeAfterStep1StepC))]
    public class CyclicBeforeAfterStep1StepA : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }
    public class CyclicBeforeAfterStep1StepB : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [AfterStep(typeof(CyclicBeforeAfterStep1StepB))]
    public class CyclicBeforeAfterStep1StepC : Step<object>
    {
        public override IEnumerable<IStepPhase> Phases => StepPhase.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }
}
