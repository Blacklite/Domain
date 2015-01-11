using Blacklite.Framework.Domain.Process;
using Blacklite.Framework.Domain.Process.Steps;
using Blacklite.Framework.Steps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Process.Tests.Fixtures
{
    [BeforeStep(typeof(StepInitPhases))]
    public class StepInit : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(object instance)
        {
        }
    }

    [BeforeStep(typeof(StepAllPhases))]
    public class StepPostInit : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.PostInit;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [AfterStep(typeof(StepAllPhases))]
    public class StepPreInit : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.PreInit;

        public IEnumerable<IValidation> Execute(object instance)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    public class StepInitPhases : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.InitPhases;

        public IEnumerable<IValidation> Execute(IProcessContext context, object instance)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    [BeforeStep(typeof(StepAllPhases))]
    public class StepPreSave : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.PreSave;
        public IEnumerable<IValidation> Execute(object instance, IProcessContext context)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    public interface IInjectable { }

    [BeforeStep(typeof(StepAllPhases))]
    public class StepSave : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Save;
        public void Execute(object instance, IProcessContext context, IInjectable injectable)
        {
        }
    }

    [BeforeStep(typeof(StepAllPhases))]
    public class StepValidate : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Validate;
        public IEnumerable<IValidation> Execute(object instance, IInjectable injectable)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    [BeforeStep(typeof(StepSavePhases))]
    [BeforeStep(typeof(StepAllPhases))]
    public class StepPostSave : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.PostSave;
        public void Execute(object instance, IInjectable injectable)
        {
        }
    }
    public class StepSavePhases : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.SavePhases;
        public IEnumerable<IValidation> Execute(object instance, IProcessContext context, IInjectable injectable)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    [AfterStep(typeof(StepInitPhases))]
    [AfterStep(typeof(StepSavePhases))]
    public class StepAllPhases : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.AllPhases;
        public IEnumerable<IValidation> Execute(object instance, IProcessContext context, IInjectable injectable)
        {
            return Enumerable.Empty<IValidation>();
        }
    }

    public abstract class StepVoidExecute : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public abstract void Execute(object instance);
    }

    public abstract class StepVoidExecuteContext : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public abstract void Execute(object instance, IProcessContext context);
    }

    public abstract class StepVoidExecuteInjectable : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public abstract void Execute(object instance, IProcessContext context, IInjectable injectable);
    }

    public abstract class StepValidationExecute : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public abstract IEnumerable<IValidation> Execute(object instance);
    }

    public abstract class StepValidationExecuteContext : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public abstract IEnumerable<IValidation> Execute(object instance, IProcessContext context);
    }

    public abstract class StepValidationExecuteInjectable : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public abstract IEnumerable<IValidation> Execute(object instance, IProcessContext context, IInjectable injectable);
    }

    [BeforeStep(typeof(CyclicBefore1StepB))]
    public class CyclicBefore1StepA : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(object instance)
        {
        }
    }

    [BeforeStep(typeof(CyclicBefore1StepA))]
    public class CyclicBefore1StepB : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [BeforeStep(typeof(CyclicBefore2StepB))]
    public class CyclicBefore2StepA : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [BeforeStep(typeof(CyclicBefore2StepC))]
    public class CyclicBefore2StepB : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [BeforeStep(typeof(CyclicBefore2StepA))]
    public class CyclicBefore2StepC : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter1StepB))]
    public class CyclicAfter1StepA : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(object instance)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter1StepA))]
    public class CyclicAfter1StepB : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter2StepB))]
    public class CyclicAfter2StepA : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter2StepC))]
    public class CyclicAfter2StepB : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [AfterStep(typeof(CyclicAfter2StepA))]
    public class CyclicAfter2StepC : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [BeforeStep(typeof(CyclicBeforeAfterStep1StepB))]
    [AfterStep(typeof(CyclicBeforeAfterStep1StepC))]
    public class CyclicBeforeAfterStep1StepA : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }
    public class CyclicBeforeAfterStep1StepB : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }

    [AfterStep(typeof(CyclicBeforeAfterStep1StepB))]
    public class CyclicBeforeAfterStep1StepC : DomainStep<object>
    {
        public override IEnumerable<IStepPhase> Phases => DomainPhases.Init;

        public void Execute(IProcessContext context, object instance)
        {
        }
    }
}
