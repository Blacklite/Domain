using System;

namespace Blacklite.Framework.Domain.Process.Steps
{
    enum StepStage
    {
        Ignore,
        Init,
        Save
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    class StepStageAttribute : Attribute
    {
        public StepStageAttribute(StepStage stage)
        {
            Stage = stage;
        }

        public StepStage Stage { get; }
    }

    [Flags]
    public enum StepPhase
    {
        [StepStage(StepStage.Init)]
        PreInit = 1 << 0,
        [StepStage(StepStage.Init)]
        Init = 1 << 1,
        [StepStage(StepStage.Init)]
        PostInit = 1 << 2,
        [StepStage(StepStage.Save)]
        PreSave = 1 << 3,
        [StepStage(StepStage.Save)]
        Validate = 1 << 4,
        [StepStage(StepStage.Save)]
        Save = 1 << 5,
        [StepStage(StepStage.Save)]
        PostSave = 1 << 6,
        // Shortcuts
        [StepStage(StepStage.Ignore)]
        InitPhases = Init | PreInit | PostInit,
        [StepStage(StepStage.Ignore)]
        SavePhases = PostSave | PreSave | Save | Validate,
        [StepStage(StepStage.Ignore)]
        AllPhases = InitPhases | SavePhases
    }
}
