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
}
