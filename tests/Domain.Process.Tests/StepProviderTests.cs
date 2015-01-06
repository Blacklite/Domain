using System;
using Xunit;
using Moq;
using Blacklite.Framework.Domain.Process;
using Domain.Process.Tests.Fixtures;
using System.Linq;

namespace Domain.Process.Tests
{
    public class StepProviderTests
    {
        [Fact]
        public void ResolvesStepsForInitStepPhase()
        {
            var stepPreInit = new StepPreInit();
            var stepInit = new StepInit();
            var stepPostInit = new StepPostInit();
            var stepPreSave = new StepPreSave();
            var stepValidate = new StepValidate();
            var stepSave = new StepSave();
            var stepPostSave = new StepPostSave();
            var stepInitPhases = new StepInitPhases();
            var stepSavePhases = new StepSavePhases();
            var stepAllPhases = new StepAllPhases();
            var mockSteps = new IStep[]
            {
                stepPreInit,
                stepInit,
                stepPostInit,
                stepPreSave,
                stepValidate,
                stepSave,
                stepPostSave,
                stepInitPhases,
                stepSavePhases,
                stepAllPhases
            };

            var provider = new StepProvider(mockSteps);

            var steps = provider.GetInitSteps<object>(null).SelectMany(x => x.Value).Distinct();

            //Assert.Equal(5, steps.Count());

            var underlyingSteps = steps.Cast<StepDescriptor>().Select(x => x.Step);

            Assert.Contains(stepPreInit, underlyingSteps);
            Assert.Contains(stepInit, underlyingSteps);
            Assert.Contains(stepPostInit, underlyingSteps);
            Assert.DoesNotContain(stepPreSave, underlyingSteps);
            Assert.DoesNotContain(stepValidate, underlyingSteps);
            Assert.DoesNotContain(stepSave, underlyingSteps);
            Assert.DoesNotContain(stepPostSave, underlyingSteps);
            Assert.Contains(stepInitPhases, underlyingSteps);
            Assert.DoesNotContain(stepSavePhases, underlyingSteps);
            Assert.Contains(stepAllPhases, underlyingSteps);

            steps = provider.GetInitSteps(new object()).SelectMany(x => x.Value).Distinct();

            //Assert.Equal(5, steps.Count());

            underlyingSteps = steps.Cast<StepDescriptor>().Select(x => x.Step);

            Assert.Contains(stepPreInit, underlyingSteps);
            Assert.Contains(stepInit, underlyingSteps);
            Assert.Contains(stepPostInit, underlyingSteps);
            Assert.DoesNotContain(stepPreSave, underlyingSteps);
            Assert.DoesNotContain(stepValidate, underlyingSteps);
            Assert.DoesNotContain(stepSave, underlyingSteps);
            Assert.DoesNotContain(stepPostSave, underlyingSteps);
            Assert.Contains(stepInitPhases, underlyingSteps);
            Assert.DoesNotContain(stepSavePhases, underlyingSteps);
            Assert.Contains(stepAllPhases, underlyingSteps);
        }

        [Fact]
        public void ResolvesStepsForSaveStepPhase()
        {
            var stepPreInit = new StepPreInit();
            var stepInit = new StepInit();
            var stepPostInit = new StepPostInit();
            var stepPreSave = new StepPreSave();
            var stepValidate = new StepValidate();
            var stepSave = new StepSave();
            var stepPostSave = new StepPostSave();
            var stepInitPhases = new StepInitPhases();
            var stepSavePhases = new StepSavePhases();
            var stepAllPhases = new StepAllPhases();
            var mockSteps = new IStep[]
            {
                stepPreInit,
                stepInit,
                stepPostInit,
                stepPreSave,
                stepValidate,
                stepSave,
                stepPostSave,
                stepInitPhases,
                stepSavePhases,
                stepAllPhases
            };

            var provider = new StepProvider(mockSteps);

            var steps = provider.GetSaveSteps<object>(null).SelectMany(x => x.Value).Distinct();

            Assert.Equal(6, steps.Count());

            var underlyingSteps = steps.Cast<StepDescriptor>().Select(x => x.Step);

            Assert.DoesNotContain(stepPreInit, underlyingSteps);
            Assert.DoesNotContain(stepInit, underlyingSteps);
            Assert.DoesNotContain(stepPostInit, underlyingSteps);
            Assert.Contains(stepPreSave, underlyingSteps);
            Assert.Contains(stepValidate, underlyingSteps);
            Assert.Contains(stepSave, underlyingSteps);
            Assert.Contains(stepPostSave, underlyingSteps);
            Assert.DoesNotContain(stepInitPhases, underlyingSteps);
            Assert.Contains(stepSavePhases, underlyingSteps);
            Assert.Contains(stepAllPhases, underlyingSteps);

            steps = provider.GetSaveSteps(new object()).SelectMany(x => x.Value).Distinct();

            Assert.Equal(6, steps.Count());

            underlyingSteps = steps.Cast<StepDescriptor>().Select(x => x.Step);

            Assert.DoesNotContain(stepPreInit, underlyingSteps);
            Assert.DoesNotContain(stepInit, underlyingSteps);
            Assert.DoesNotContain(stepPostInit, underlyingSteps);
            Assert.Contains(stepPreSave, underlyingSteps);
            Assert.Contains(stepValidate, underlyingSteps);
            Assert.Contains(stepSave, underlyingSteps);
            Assert.Contains(stepPostSave, underlyingSteps);
            Assert.DoesNotContain(stepInitPhases, underlyingSteps);
            Assert.Contains(stepSavePhases, underlyingSteps);
            Assert.Contains(stepAllPhases, underlyingSteps);
        }

        [Fact]
        public void ResolvesStepsForEachPhase()
        {
            var stepPreInit = new StepPreInit();
            var stepInit = new StepInit();
            var stepPostInit = new StepPostInit();
            var stepPreSave = new StepPreSave();
            var stepValidate = new StepValidate();
            var stepSave = new StepSave();
            var stepPostSave = new StepPostSave();
            var stepInitPhases = new StepInitPhases();
            var stepSavePhases = new StepSavePhases();
            var stepAllPhases = new StepAllPhases();
            var mockSteps = new IStep[]
            {
                stepPreInit,
                stepInit,
                stepPostInit,
                stepPreSave,
                stepValidate,
                stepSave,
                stepPostSave,
                stepInitPhases,
                stepSavePhases,
                stepAllPhases
            };

            var provider = new StepProvider(mockSteps);

            var steps = provider.GetStepsForPhase(StepPhase.PreInit, new object());

            //Assert.Equal(3, steps.Count());

            var underlyingSteps = steps.Cast<StepDescriptor>().Select(x => x.Step);

            Assert.Contains(stepPreInit, underlyingSteps);
            Assert.DoesNotContain(stepInit, underlyingSteps);
            Assert.DoesNotContain(stepPostInit, underlyingSteps);
            Assert.DoesNotContain(stepPreSave, underlyingSteps);
            Assert.DoesNotContain(stepValidate, underlyingSteps);
            Assert.DoesNotContain(stepSave, underlyingSteps);
            Assert.DoesNotContain(stepPostSave, underlyingSteps);
            Assert.Contains(stepInitPhases, underlyingSteps);
            Assert.DoesNotContain(stepSavePhases, underlyingSteps);
            Assert.Contains(stepAllPhases, underlyingSteps);

            steps = provider.GetStepsForPhase(StepPhase.Init, new object());

            //Assert.Equal(3, steps.Count());

            underlyingSteps = steps.Cast<StepDescriptor>().Select(x => x.Step);

            Assert.DoesNotContain(stepPreInit, underlyingSteps);
            Assert.Contains(stepInit, underlyingSteps);
            Assert.DoesNotContain(stepPostInit, underlyingSteps);
            Assert.DoesNotContain(stepPreSave, underlyingSteps);
            Assert.DoesNotContain(stepValidate, underlyingSteps);
            Assert.DoesNotContain(stepSave, underlyingSteps);
            Assert.DoesNotContain(stepPostSave, underlyingSteps);
            Assert.Contains(stepInitPhases, underlyingSteps);
            Assert.DoesNotContain(stepSavePhases, underlyingSteps);
            Assert.Contains(stepAllPhases, underlyingSteps);

            steps = provider.GetStepsForPhase(StepPhase.PostInit, new object());

            //Assert.Equal(3, steps.Count());

            underlyingSteps = steps.Cast<StepDescriptor>().Select(x => x.Step);

            Assert.DoesNotContain(stepPreInit, underlyingSteps);
            Assert.DoesNotContain(stepInit, underlyingSteps);
            Assert.Contains(stepPostInit, underlyingSteps);
            Assert.DoesNotContain(stepPreSave, underlyingSteps);
            Assert.DoesNotContain(stepValidate, underlyingSteps);
            Assert.DoesNotContain(stepSave, underlyingSteps);
            Assert.DoesNotContain(stepPostSave, underlyingSteps);
            Assert.Contains(stepInitPhases, underlyingSteps);
            Assert.DoesNotContain(stepSavePhases, underlyingSteps);
            Assert.Contains(stepAllPhases, underlyingSteps);

            steps = provider.GetStepsForPhase(StepPhase.PreSave, new object());

            //Assert.Equal(3, steps.Count());

            underlyingSteps = steps.Cast<StepDescriptor>().Select(x => x.Step);

            Assert.DoesNotContain(stepPreInit, underlyingSteps);
            Assert.DoesNotContain(stepInit, underlyingSteps);
            Assert.DoesNotContain(stepPostInit, underlyingSteps);
            Assert.Contains(stepPreSave, underlyingSteps);
            Assert.DoesNotContain(stepValidate, underlyingSteps);
            Assert.DoesNotContain(stepSave, underlyingSteps);
            Assert.DoesNotContain(stepPostSave, underlyingSteps);
            Assert.DoesNotContain(stepInitPhases, underlyingSteps);
            Assert.Contains(stepSavePhases, underlyingSteps);
            Assert.Contains(stepAllPhases, underlyingSteps);

            steps = provider.GetStepsForPhase(StepPhase.Validate, new object());

            //Assert.Equal(3, steps.Count());

            underlyingSteps = steps.Cast<StepDescriptor>().Select(x => x.Step);

            Assert.DoesNotContain(stepPreInit, underlyingSteps);
            Assert.DoesNotContain(stepInit, underlyingSteps);
            Assert.DoesNotContain(stepPostInit, underlyingSteps);
            Assert.DoesNotContain(stepPreSave, underlyingSteps);
            Assert.Contains(stepValidate, underlyingSteps);
            Assert.DoesNotContain(stepSave, underlyingSteps);
            Assert.DoesNotContain(stepPostSave, underlyingSteps);
            Assert.DoesNotContain(stepInitPhases, underlyingSteps);
            Assert.Contains(stepSavePhases, underlyingSteps);
            Assert.Contains(stepAllPhases, underlyingSteps);

            steps = provider.GetStepsForPhase(StepPhase.Save, new object());

            //Assert.Equal(3, steps.Count());

            underlyingSteps = steps.Cast<StepDescriptor>().Select(x => x.Step);

            Assert.DoesNotContain(stepPreInit, underlyingSteps);
            Assert.DoesNotContain(stepInit, underlyingSteps);
            Assert.DoesNotContain(stepPostInit, underlyingSteps);
            Assert.DoesNotContain(stepPreSave, underlyingSteps);
            Assert.DoesNotContain(stepValidate, underlyingSteps);
            Assert.Contains(stepSave, underlyingSteps);
            Assert.DoesNotContain(stepPostSave, underlyingSteps);
            Assert.DoesNotContain(stepInitPhases, underlyingSteps);
            Assert.Contains(stepSavePhases, underlyingSteps);
            Assert.Contains(stepAllPhases, underlyingSteps);

            steps = provider.GetStepsForPhase(StepPhase.PostSave, new object());

            //Assert.Equal(3, steps.Count());

            underlyingSteps = steps.Cast<StepDescriptor>().Select(x => x.Step);

            Assert.DoesNotContain(stepPreInit, underlyingSteps);
            Assert.DoesNotContain(stepInit, underlyingSteps);
            Assert.DoesNotContain(stepPostInit, underlyingSteps);
            Assert.DoesNotContain(stepPreSave, underlyingSteps);
            Assert.DoesNotContain(stepValidate, underlyingSteps);
            Assert.DoesNotContain(stepSave, underlyingSteps);
            Assert.Contains(stepPostSave, underlyingSteps);
            Assert.DoesNotContain(stepInitPhases, underlyingSteps);
            Assert.Contains(stepSavePhases, underlyingSteps);
            Assert.Contains(stepAllPhases, underlyingSteps);

            steps = provider.GetStepsForPhase(StepPhase.InitPhases, new object());
            Assert.Equal(0, steps.Count());

            steps = provider.GetStepsForPhase(StepPhase.SavePhases, new object());
            Assert.Equal(0, steps.Count());

            steps = provider.GetStepsForPhase(StepPhase.AllPhases, new object());
            Assert.Equal(0, steps.Count());
        }

        [Fact]
        public void StepsOrderCorrectly()
        {
            var stepPreInit = new StepPreInit();
            var stepInit = new StepInit();
            var stepPostInit = new StepPostInit();
            var stepPreSave = new StepPreSave();
            var stepValidate = new StepValidate();
            var stepSave = new StepSave();
            var stepPostSave = new StepPostSave();
            var stepInitPhases = new StepInitPhases();
            var stepSavePhases = new StepSavePhases();
            var stepAllPhases = new StepAllPhases();
            var mockSteps = new IStep[]
            {
                stepPreInit,
                stepInit,
                stepPostInit,
                stepPreSave,
                stepValidate,
                stepSave,
                stepPostSave,
                stepInitPhases,
                stepSavePhases,
                stepAllPhases
            };

            var provider = new StepProvider(mockSteps);

            var steps = provider.GetStepsForPhase(StepPhase.PreInit, new object()).Cast<StepDescriptor>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepInitPhases);
            Assert.Same(steps[1], stepAllPhases);
            Assert.Same(steps[2], stepPreInit);

            steps = provider.GetStepsForPhase(StepPhase.Init, new object()).Cast<StepDescriptor>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepInit);
            Assert.Same(steps[1], stepInitPhases);
            Assert.Same(steps[2], stepAllPhases);

            steps = provider.GetStepsForPhase(StepPhase.PostInit, new object()).Cast<StepDescriptor>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepInitPhases);
            Assert.Same(steps[1], stepPostInit);
            Assert.Same(steps[2], stepAllPhases);

            steps = provider.GetStepsForPhase(StepPhase.PreSave, new object()).Cast<StepDescriptor>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepSavePhases);
            Assert.Same(steps[1], stepPreSave);
            Assert.Same(steps[2], stepAllPhases);

            steps = provider.GetStepsForPhase(StepPhase.Validate, new object()).Cast<StepDescriptor>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepSavePhases);
            Assert.Same(steps[1], stepValidate);
            Assert.Same(steps[2], stepAllPhases);

            steps = provider.GetStepsForPhase(StepPhase.Save, new object()).Cast<StepDescriptor>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepSavePhases);
            Assert.Same(steps[1], stepSave);
            Assert.Same(steps[2], stepAllPhases);

            steps = provider.GetStepsForPhase(StepPhase.PostSave, new object()).Cast<StepDescriptor>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepPostSave);
            Assert.Same(steps[1], stepSavePhases);
            Assert.Same(steps[2], stepAllPhases);
        }
    }
}
