using Blacklite.Framework.Steps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blacklite.Framework.Domain.Process.Steps
{
    public static class StepProviderExtensions
    {
        public static string Init { get; } = nameof(Init);
        public static string Save { get; } = nameof(Save);

        public static IEnumerable<IGrouping<IStepPhase, IStepDescriptor<IEnumerable<IValidation>>>> GetInitSteps<T>(
            [NotNull] this IDomainStepProvider provider,
            [NotNull] IProcessContext context,
            [NotNull] T instance) where T : class
        {
            return provider.GetStepsForStage(Init, context, instance);
        }

        public static IEnumerable<IGrouping<IStepPhase, IStepDescriptor<IEnumerable<IValidation>>>> GetSaveSteps<T>(
            [NotNull] this IDomainStepProvider provider,
            [NotNull] IProcessContext context,
            [NotNull] T instance) where T : class
        {
            return provider.GetStepsForStage(Save, context, instance);
        }
    }
}
