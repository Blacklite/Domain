using System;
using Xunit;
using Moq;
using Blacklite.Framework.Domain.Process;
using Domain.Process.Tests.Fixtures;
using System.Linq;
using Microsoft.AspNet.Http;

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

        [Fact]
        public void StepsExecuteProperty()
        {

            var context = new object();
            var httpContextMock = new Mock<HttpContext>();
            var httpContext = httpContextMock.Object;

            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceProvider = serviceProviderMock.Object;

            var injectableMock = new Mock<IInjectable>();
            var injectable = injectableMock.Object;

            serviceProviderMock.Setup(x => x.GetService(typeof(IInjectable))).Returns(injectableMock.Object);
            httpContextMock.SetupGet(x => x.RequestServices).Returns(serviceProviderMock.Object);

            var voidExecuteMock = new Mock<StepVoidExecute>();
            var voidExecuteContextMock = new Mock<StepVoidExecuteContext>();
            var voidExecuteInjectableMock = new Mock<StepVoidExecuteInjectable>();
            voidExecuteMock.Setup(x => x.Execute(context));
            voidExecuteMock.SetupGet(x => x.Phase).Returns(StepPhase.Init);
            voidExecuteMock.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            voidExecuteMock.Setup(x => x.CanExecute(context, httpContext)).Returns(true);

            voidExecuteContextMock.Setup(x => x.Execute(context, httpContext));
            voidExecuteContextMock.SetupGet(x => x.Phase).Returns(StepPhase.Init);
            voidExecuteContextMock.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            voidExecuteContextMock.Setup(x => x.CanExecute(context, httpContext)).Returns(true);

            voidExecuteInjectableMock.Setup(x => x.Execute(context, httpContext, injectable));
            voidExecuteInjectableMock.SetupGet(x => x.Phase).Returns(StepPhase.Init);
            voidExecuteInjectableMock.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            voidExecuteInjectableMock.Setup(x => x.CanExecute(context, httpContext)).Returns(true);


            var validationExecuteMock = new Mock<StepValidationExecute>();
            var validationExecuteContextMock = new Mock<StepValidationExecuteContext>();
            var validationExecuteInjectableMock = new Mock<StepValidationExecuteInjectable>();
            validationExecuteMock.Setup(x => x.Execute(context));
            validationExecuteMock.SetupGet(x => x.Phase).Returns(StepPhase.Init);
            validationExecuteMock.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            validationExecuteMock.Setup(x => x.CanExecute(It.IsAny<object>(), It.IsAny<HttpContext>())).Returns(true);

            validationExecuteContextMock.Setup(x => x.Execute(context, httpContext));
            validationExecuteContextMock.SetupGet(x => x.Phase).Returns(StepPhase.Init);
            validationExecuteContextMock.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            validationExecuteContextMock.Setup(x => x.CanExecute(It.IsAny<object>(), It.IsAny<HttpContext>())).Returns(true);

            validationExecuteInjectableMock.Setup(x => x.Execute(context, httpContext, injectable));
            validationExecuteInjectableMock.SetupGet(x => x.Phase).Returns(StepPhase.Init);
            validationExecuteInjectableMock.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            validationExecuteInjectableMock.Setup(x => x.CanExecute(It.IsAny<object>(), It.IsAny<HttpContext>())).Returns(true);

            var mockSteps = new IStep[]
            {
                voidExecuteMock.Object,
                voidExecuteContextMock.Object,
                voidExecuteInjectableMock.Object,
                validationExecuteMock.Object,
                validationExecuteContextMock.Object,
                validationExecuteInjectableMock.Object
            };

            var provider = new StepProvider(mockSteps);

            var steps = provider.GetStepsForPhase(StepPhase.Init, context, httpContext);

            foreach (var step in steps)
            {
                step.Execute(context, serviceProvider, httpContext);
            }

            voidExecuteMock.Verify(x => x.Execute(context));
            voidExecuteContextMock.Verify(x => x.Execute(context, httpContext));
            voidExecuteInjectableMock.Verify(x => x.Execute(context, httpContext, injectable));

            validationExecuteMock.Verify(x => x.Execute(context));
            validationExecuteContextMock.Verify(x => x.Execute(context, httpContext));
            validationExecuteInjectableMock.Verify(x => x.Execute(context, httpContext, injectable));
        }

        [Fact]
        public void StepsSupportsVariableCanExecuteDefinitions()
        {

            var context = new object();
            var httpContextMock = new Mock<HttpContext>();
            var httpContext = httpContextMock.Object;

            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceProvider = serviceProviderMock.Object;

            var injectableMock = new Mock<IInjectable>();
            var injectable = injectableMock.Object;

            serviceProviderMock.Setup(x => x.GetService(typeof(IInjectable))).Returns(injectableMock.Object);
            httpContextMock.SetupGet(x => x.RequestServices).Returns(serviceProviderMock.Object);

            var customStepCanExecute = new Mock<CustomStepCanExecute>();
            var customStepCanExecuteContext = new Mock<CustomStepCanExecuteContext>();
            var customStepCanExecuteContext2 = new Mock<CustomStepCanExecuteContext2>();
            customStepCanExecute.SetupGet(x => x.Phase).Returns(StepPhase.Init);
            customStepCanExecute.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            customStepCanExecute.Setup(x => x.CanExecute(context)).Returns(true);
            customStepCanExecute.Setup(x => x.Execute(context)).Returns(Enumerable.Empty<IValidation>());

            customStepCanExecuteContext.SetupGet(x => x.Phase).Returns(StepPhase.Init);
            customStepCanExecuteContext.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            customStepCanExecuteContext.Setup(x => x.CanExecute(It.IsAny<HttpContext>(), context)).Returns(true);
            customStepCanExecuteContext.Setup(x => x.Execute(context)).Returns(Enumerable.Empty<IValidation>());

            customStepCanExecuteContext2.SetupGet(x => x.Phase).Returns(StepPhase.Init);
            customStepCanExecuteContext2.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            customStepCanExecuteContext2.Setup(x => x.CanExecute(context, It.IsAny<HttpContext>())).Returns(true);
            customStepCanExecuteContext2.Setup(x => x.Execute(context)).Returns(Enumerable.Empty<IValidation>());

            var mockSteps = new IStep[]
            {
                customStepCanExecute.Object,
                customStepCanExecuteContext.Object,
                customStepCanExecuteContext2.Object,
            };

            var provider = new StepProvider(mockSteps);

            var steps = provider.GetStepsForPhase(StepPhase.Init, context, httpContext).ToArray();

            customStepCanExecute.Verify(x => x.CanExecute(context));
            customStepCanExecuteContext.Verify(x => x.CanExecute(httpContext, context));
            customStepCanExecuteContext2.Verify(x => x.CanExecute(context, httpContext));

            steps = provider.GetStepsForPhase(StepPhase.Init, context).ToArray();

            customStepCanExecute.Verify(x => x.CanExecute(context));
            customStepCanExecuteContext.Verify(x => x.CanExecute(null, context));
            customStepCanExecuteContext2.Verify(x => x.CanExecute(context, null));
        }

        [Fact]
        public void CanExecuteIsNotInjectable()
        {
            var context = new object();
            var httpContextMock = new Mock<HttpContext>();
            var httpContext = httpContextMock.Object;

            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceProvider = serviceProviderMock.Object;

            var injectableMock = new Mock<IInjectable>();
            var injectable = injectableMock.Object;

            serviceProviderMock.Setup(x => x.GetService(typeof(IInjectable))).Returns(injectableMock.Object);
            httpContextMock.SetupGet(x => x.RequestServices).Returns(serviceProviderMock.Object);

            var customStepCanExecuteInvalid = new Mock<CustomStepCanExecuteInvalid>();
            customStepCanExecuteInvalid.SetupGet(x => x.Phase).Returns(StepPhase.Init);
            customStepCanExecuteInvalid.Setup(x => x.CanRun(It.IsAny<Type>())).Returns(true);
            customStepCanExecuteInvalid.Setup(x => x.CanExecute(context, httpContext, new Mock<IInjectable>().Object)).Returns(true);

            var mockSteps = new IStep[]
            {
                customStepCanExecuteInvalid.Object,
            };

            Assert.Throws(typeof(NotSupportedException), () => new StepProvider(mockSteps));
        }

        [Fact]
        public void DetectsCyclicDependencies()
        {
            IStep[] mockSteps;

            mockSteps = new IStep[]
            {
                new CyclicBefore1StepA(),
                new CyclicBefore1StepB(),
            };

            Assert.Throws(typeof(NotSupportedException), () => new StepProvider(mockSteps));

            mockSteps = new IStep[]
            {
                new CyclicBefore2StepA(),
                new CyclicBefore2StepB(),
                new CyclicBefore2StepC(),
            };

            Assert.Throws(typeof(NotSupportedException), () => new StepProvider(mockSteps));

            mockSteps = new IStep[]
            {
                new CyclicAfter1StepA(),
                new CyclicAfter1StepB(),
            };

            Assert.Throws(typeof(NotSupportedException), () => new StepProvider(mockSteps));

            mockSteps = new IStep[]
            {
                new CyclicAfter2StepA(),
                new CyclicAfter2StepB(),
                new CyclicAfter2StepC(),
            };

            Assert.Throws(typeof(NotSupportedException), () => new StepProvider(mockSteps));

            mockSteps = new IStep[]
            {
                new CyclicBeforeAfterStep1StepA(),
                new CyclicBeforeAfterStep1StepB(),
                new CyclicBeforeAfterStep1StepC(),
            };

            Assert.Throws(typeof(NotSupportedException), () => new StepProvider(mockSteps));
        }
    }
}
