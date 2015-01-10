using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Blacklite.Framework.Domain.Process.Steps
{
    public interface IStepPhase
    {
        string Stage { get; }
        int Order { get; }
    }

    public class StepPhase : IStepPhase, IEnumerable<IStepPhase>
    {
        private readonly string _name;

        public StepPhase(string name, int order, string stage)
        {
            _name = name;
            Order = order;
            Stage = stage;
        }

        public string Stage { get; }

        /// <summary>
        /// 0 is first
        /// </summary>
        public int Order { get; }

        static StepPhase()
        {
            PreInit = new StepPhase(nameof(PreInit), 0, StepProviderExtensions.Init);
            Init = new StepPhase(nameof(Init), 1, StepProviderExtensions.Init);
            PostInit = new StepPhase(nameof(PostInit), 2, StepProviderExtensions.Init);
            InitPhases = PreInit.Union(Init).Union(PostInit);

            PreSave = new StepPhase(nameof(PreSave), 0, StepProviderExtensions.Save);
            Validate = new StepPhase(nameof(Validate), 1, StepProviderExtensions.Save);
            Save = new StepPhase(nameof(Save), 2, StepProviderExtensions.Save);
            PostSave = new StepPhase(nameof(PostSave), 3, StepProviderExtensions.Save);
            SavePhases = PreSave.Union(Validate).Union(Save).Union(PostSave);

            AllPhases = InitPhases.Union(SavePhases);
        }

        public override string ToString() => string.Format("[{0}]{1}", Stage, _name);

        public IEnumerator<IStepPhase> GetEnumerator()
        {
            return new[] { this }.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static StepPhase PreInit { get; }
        public static StepPhase Init { get; }
        public static StepPhase PostInit { get; }
        public static IEnumerable<IStepPhase> InitPhases { get; }

        public static StepPhase PreSave { get; }
        public static StepPhase Validate { get; }
        public static StepPhase Save { get; }
        public static StepPhase PostSave { get; }

        public static IEnumerable<IStepPhase> SavePhases { get; }

        public static IEnumerable<IStepPhase> AllPhases { get; }
    }
}
