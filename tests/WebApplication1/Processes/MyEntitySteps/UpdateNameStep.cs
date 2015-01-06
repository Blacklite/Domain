using Blacklite.Framework.Domain.Process.Steps;
using Microsoft.Framework.DependencyInjection;
using System;
using WebApplication1.Controllers;

namespace WebApplication1.Processes.MyEntitySteps
{
    [ServiceDescriptor(typeof(IStep))]
    public class UpdateNameStep : Step<MyEntity>
    {
        public override StepPhase Phase { get; } = StepPhase.PreSave;

        public void Execute(MyEntity entity)
        {
            entity.Name = (entity.Name ?? "") + "123";
        }
    }
}
