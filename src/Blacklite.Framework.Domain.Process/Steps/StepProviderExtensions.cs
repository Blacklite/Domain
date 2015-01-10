using System;
using System.Collections.Generic;
using System.Linq;

namespace Blacklite.Framework.Domain.Process.Steps
{
    public static class StepProviderExtensions
    {
        public static string Init { get; } = nameof(Init);
        public static string Save { get; } = nameof(Save);

        public static IEnumerable<IGrouping<IStepPhase, IStepDescriptor>> GetInitSteps<T>(
            [NotNull] this IStepProvider provider,
            [NotNull] T instance,
            [NotNull]IProcessContext context) where T : class
        {
            return provider.GetStepsForStage(Init, instance, context);
        }

        public static IEnumerable<IGrouping<IStepPhase, IStepDescriptor>> GetSaveSteps<T>(
            [NotNull] this IStepProvider provider,
            [NotNull] T instance,
            [NotNull]IProcessContext context) where T : class
        {
            return provider.GetStepsForStage(Save, instance, context);
        }
    }
}
