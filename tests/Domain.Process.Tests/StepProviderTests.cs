using System;
using Xunit;
using Moq;
using Blacklite.Framework.Domain.Process;
using Domain.Process.Tests.Fixtures;
using System.Linq;
using Blacklite.Framework.Steps;
using Blacklite.Framework.Domain.Process.Steps;
using System.Collections.Generic;

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
            var mockSteps = new IDomainStep[]
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

            var provider = new DomainStepProvider(new StepCache<,>(mockSteps));

            var steps = provider.GetInitSteps<object>(null, It.IsAny<IProcessContext>()).SelectMany(x => x).Distinct();

            //Assert.Equal(5, steps.Count());

            var underlyingSteps = steps.Cast< IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step);

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

            steps = provider.GetInitSteps(new object(), It.IsAny<IProcessContext>()).SelectMany(x => x).Distinct();

            //Assert.Equal(5, steps.Count());

            underlyingSteps = steps.Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step);

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
            var mockSteps = new IDomainStep[]
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

            var provider = new DomainStepProvider(mockSteps);

            var steps = provider.GetSaveSteps<object>(null, It.IsAny<IProcessContext>()).SelectMany(x => x).Distinct();

            Assert.Equal(6, steps.Count());

            var underlyingSteps = steps.Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step);

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

            steps = provider.GetSaveSteps(new object(), It.IsAny<IProcessContext>()).SelectMany(x => x).Distinct();

            Assert.Equal(6, steps.Count());

            underlyingSteps = steps.Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step);

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
            var mockSteps = new IDomainStep[]
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

            var provider = new DomainStepProvider(mockSteps);

            var steps = provider.GetStepsForPhase(StepPhase.PreInit, new object(), It.IsAny<IProcessContext>());

            //Assert.Equal(3, steps.Count());

            var underlyingSteps = steps.Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step);

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

            steps = provider.GetStepsForPhase(StepPhase.Init, new object(), It.IsAny<IProcessContext>());

            //Assert.Equal(3, steps.Count());

            underlyingSteps = steps.Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step);

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

            steps = provider.GetStepsForPhase(StepPhase.PostInit, new object(), It.IsAny<IProcessContext>());

            //Assert.Equal(3, steps.Count());

            underlyingSteps = steps.Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step);

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

            steps = provider.GetStepsForPhase(StepPhase.PreSave, new object(), It.IsAny<IProcessContext>());

            //Assert.Equal(3, steps.Count());

            underlyingSteps = steps.Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step);

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

            steps = provider.GetStepsForPhase(StepPhase.Validate, new object(), It.IsAny<IProcessContext>());

            //Assert.Equal(3, steps.Count());

            underlyingSteps = steps.Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step);

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

            steps = provider.GetStepsForPhase(StepPhase.Save, new object(), It.IsAny<IProcessContext>());

            //Assert.Equal(3, steps.Count());

            underlyingSteps = steps.Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step);

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

            steps = provider.GetStepsForPhase(StepPhase.PostSave, new object(), It.IsAny<IProcessContext>());

            //Assert.Equal(3, steps.Count());

            underlyingSteps = steps.Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step);

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
            var mockSteps = new IDomainStep[]
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

            var provider = new DomainStepProvider(mockSteps);

            var steps = provider.GetStepsForPhase(StepPhase.PreInit, new object(), It.IsAny<IProcessContext>()).Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepInitPhases);
            Assert.Same(steps[1], stepAllPhases);
            Assert.Same(steps[2], stepPreInit);

            steps = provider.GetStepsForPhase(StepPhase.Init, new object(), It.IsAny<IProcessContext>()).Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepInit);
            Assert.Same(steps[1], stepInitPhases);
            Assert.Same(steps[2], stepAllPhases);

            steps = provider.GetStepsForPhase(StepPhase.PostInit, new object(), It.IsAny<IProcessContext>()).Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepInitPhases);
            Assert.Same(steps[1], stepPostInit);
            Assert.Same(steps[2], stepAllPhases);

            steps = provider.GetStepsForPhase(StepPhase.PreSave, new object(), It.IsAny<IProcessContext>()).Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepSavePhases);
            Assert.Same(steps[1], stepPreSave);
            Assert.Same(steps[2], stepAllPhases);

            steps = provider.GetStepsForPhase(StepPhase.Validate, new object(), It.IsAny<IProcessContext>()).Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepSavePhases);
            Assert.Same(steps[1], stepValidate);
            Assert.Same(steps[2], stepAllPhases);

            steps = provider.GetStepsForPhase(StepPhase.Save, new object(), It.IsAny<IProcessContext>()).Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepSavePhases);
            Assert.Same(steps[1], stepSave);
            Assert.Same(steps[2], stepAllPhases);

            steps = provider.GetStepsForPhase(StepPhase.PostSave, new object(), It.IsAny<IProcessContext>()).Cast<IStepDescriptor<IEnumerable<IValidation>>().Select(x => x.Step).ToArray();

            Assert.Same(steps[0], stepPostSave);
            Assert.Same(steps[1], stepSavePhases);
            Assert.Same(steps[2], stepAllPhases);
        }

        [Fact]
        public void StepsExecuteProperty()
        {

            var context = new object();
            var processContextMock = new Mock<IProcessContext>();
            var processContext = processContextMock.Object;

            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceProvider = serviceProviderMock.Object;

            var injectableMock = new Mock<IInjectable>();
            var injectable = injectableMock.Object;

            serviceProviderMock.Setup(x => x.GetService(typeof(IInjectable))).Returns(injectableMock.Object);
            processContextMock.SetupGet(x => x.ProcessServices).Returns(serviceProviderMock.Object);

            var voidExecuteMock = new Mock<StepVoidExecute>();
            var voidExecuteContextMock = new Mock<StepVoidExecuteContext>();
            var voidExecuteInjectableMock = new Mock<StepVoidExecuteInjectable>();
            voidExecuteMock.Setup(x => x.Execute(context));
            voidExecuteMock.SetupGet(x => x.Phases).Returns(StepPhase.Init);
            voidExecuteMock.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            voidExecuteMock.Setup(x => x.CanExecute(context, processContext)).Returns(true);

            voidExecuteContextMock.Setup(x => x.Execute(context, processContext));
            voidExecuteContextMock.SetupGet(x => x.Phases).Returns(StepPhase.Init);
            voidExecuteContextMock.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            voidExecuteContextMock.Setup(x => x.CanExecute(context, processContext)).Returns(true);

            voidExecuteInjectableMock.Setup(x => x.Execute(context, processContext, injectable));
            voidExecuteInjectableMock.SetupGet(x => x.Phases).Returns(StepPhase.Init);
            voidExecuteInjectableMock.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            voidExecuteInjectableMock.Setup(x => x.CanExecute(context, processContext)).Returns(true);


            var validationExecuteMock = new Mock<StepValidationExecute>();
            var validationExecuteContextMock = new Mock<StepValidationExecuteContext>();
            var validationExecuteInjectableMock = new Mock<StepValidationExecuteInjectable>();
            validationExecuteMock.Setup(x => x.Execute(context));
            validationExecuteMock.SetupGet(x => x.Phases).Returns(StepPhase.Init);
            validationExecuteMock.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            validationExecuteMock.Setup(x => x.CanExecute(It.IsAny<object>(), It.IsAny<IProcessContext>())).Returns(true);

            validationExecuteContextMock.Setup(x => x.Execute(context, processContext));
            validationExecuteContextMock.SetupGet(x => x.Phases).Returns(StepPhase.Init);
            validationExecuteContextMock.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            validationExecuteContextMock.Setup(x => x.CanExecute(It.IsAny<object>(), It.IsAny<IProcessContext>())).Returns(true);

            validationExecuteInjectableMock.Setup(x => x.Execute(context, processContext, injectable));
            validationExecuteInjectableMock.SetupGet(x => x.Phases).Returns(StepPhase.Init);
            validationExecuteInjectableMock.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            validationExecuteInjectableMock.Setup(x => x.CanExecute(It.IsAny<object>(), It.IsAny<IProcessContext>())).Returns(true);

            var mockSteps = new IDomainStep[]
            {
                voidExecuteMock.Object,
                voidExecuteContextMock.Object,
                voidExecuteInjectableMock.Object,
                validationExecuteMock.Object,
                validationExecuteContextMock.Object,
                validationExecuteInjectableMock.Object
            };

            var provider = new DomainStepProvider(mockSteps);

            var steps = provider.GetStepsForPhase(StepPhase.Init, context, processContext);

            foreach (var step in steps)
            {
                step.Execute(serviceProvider, context, processContext);
            }

            voidExecuteMock.Verify(x => x.Execute(context));
            voidExecuteContextMock.Verify(x => x.Execute(context, processContext));
            voidExecuteInjectableMock.Verify(x => x.Execute(context, processContext, injectable));

            validationExecuteMock.Verify(x => x.Execute(context));
            validationExecuteContextMock.Verify(x => x.Execute(context, processContext));
            validationExecuteInjectableMock.Verify(x => x.Execute(context, processContext, injectable));
        }

        [Fact]
        public void StepsSupportsVariableCanExecuteDefinitions()
        {

            var context = new object();
            var processContextMock = new Mock<IProcessContext>();
            var processContext = processContextMock.Object;

            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceProvider = serviceProviderMock.Object;

            var injectableMock = new Mock<IInjectable>();
            var injectable = injectableMock.Object;

            serviceProviderMock.Setup(x => x.GetService(typeof(IInjectable))).Returns(injectableMock.Object);
            processContextMock.SetupGet(x => x.ProcessServices).Returns(serviceProviderMock.Object);

            var customStepCanExecute = new Mock<CustomStepCanExecute>();
            var customStepCanExecuteContext = new Mock<CustomStepCanExecuteContext>();
            var customStepCanExecuteContext2 = new Mock<CustomStepCanExecuteContext2>();
            customStepCanExecute.SetupGet(x => x.Phases).Returns(StepPhase.Init);
            customStepCanExecute.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            customStepCanExecute.Setup(x => x.CanExecute(context)).Returns(true);
            customStepCanExecute.Setup(x => x.Execute(context)).Returns(Enumerable.Empty<IValidation>());

            customStepCanExecuteContext.SetupGet(x => x.Phases).Returns(StepPhase.Init);
            customStepCanExecuteContext.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            customStepCanExecuteContext.Setup(x => x.CanExecute(It.IsAny<IProcessContext>(), context)).Returns(true);
            customStepCanExecuteContext.Setup(x => x.Execute(context)).Returns(Enumerable.Empty<IValidation>());

            customStepCanExecuteContext2.SetupGet(x => x.Phases).Returns(StepPhase.Init);
            customStepCanExecuteContext2.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            customStepCanExecuteContext2.Setup(x => x.CanExecute(context, It.IsAny<IProcessContext>())).Returns(true);
            customStepCanExecuteContext2.Setup(x => x.Execute(context)).Returns(Enumerable.Empty<IValidation>());

            var mockSteps = new IDomainStep[]
            {
                customStepCanExecute.Object,
                customStepCanExecuteContext.Object,
                customStepCanExecuteContext2.Object,
            };

            var provider = new DomainStepProvider(mockSteps);

            var steps = provider.GetStepsForPhase(StepPhase.Init, context, processContext).ToArray();

            customStepCanExecute.Verify(x => x.CanExecute(context));
            customStepCanExecuteContext.Verify(x => x.CanExecute(processContext, context));
            customStepCanExecuteContext2.Verify(x => x.CanExecute(context, processContext));
        }

        [Fact]
        public void CanExecuteIsNotInjectable()
        {
            var context = new object();
            var processContextMock = new Mock<IProcessContext>();
            var processContext = processContextMock.Object;

            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceProvider = serviceProviderMock.Object;

            var injectableMock = new Mock<IInjectable>();
            var injectable = injectableMock.Object;

            serviceProviderMock.Setup(x => x.GetService(typeof(IInjectable))).Returns(injectableMock.Object);
            processContextMock.SetupGet(x => x.ProcessServices).Returns(serviceProviderMock.Object);

            var customStepCanExecuteInvalid = new Mock<CustomStepCanExecuteInvalid>();
            customStepCanExecuteInvalid.SetupGet(x => x.Phases).Returns(StepPhase.Init);
            customStepCanExecuteInvalid.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            customStepCanExecuteInvalid.Setup(x => x.CanExecute(context, processContext, new Mock<IInjectable>().Object)).Returns(true);

            var mockSteps = new IDomainStep[]
            {
                customStepCanExecuteInvalid.Object,
            };

            Assert.Throws(typeof(NotSupportedException), () => new DomainStepProvider(mockSteps));
        }

        [Fact]
        public void DetectsCyclicDependencies()
        {
            IStep[] mockSteps;

            mockSteps = new IDomainStep[]
            {
                new CyclicBefore1StepA(),
                new CyclicBefore1StepB(),
            };

            Assert.Throws(typeof(NotSupportedException), () => new DomainStepProvider(mockSteps));

            mockSteps = new IDomainStep[]
            {
                new CyclicBefore2StepA(),
                new CyclicBefore2StepB(),
                new CyclicBefore2StepC(),
            };

            Assert.Throws(typeof(NotSupportedException), () => new DomainStepProvider(mockSteps));

            mockSteps = new IDomainStep[]
            {
                new CyclicAfter1StepA(),
                new CyclicAfter1StepB(),
            };

            Assert.Throws(typeof(NotSupportedException), () => new DomainStepProvider(mockSteps));

            mockSteps = new IDomainStep[]
            {
                new CyclicAfter2StepA(),
                new CyclicAfter2StepB(),
                new CyclicAfter2StepC(),
            };

            Assert.Throws(typeof(NotSupportedException), () => new DomainStepProvider(mockSteps));

            mockSteps = new IDomainStep[]
            {
                new CyclicBeforeAfterStep1StepA(),
                new CyclicBeforeAfterStep1StepB(),
                new CyclicBeforeAfterStep1StepC(),
            };

            Assert.Throws(typeof(NotSupportedException), () => new DomainStepProvider(mockSteps));
        }
    }
}
