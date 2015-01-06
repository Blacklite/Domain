using System;

namespace Blacklite.Framework.Domain.Process.Steps
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class BeforeStepAttribute : Attribute
    {
        public Type Step { get; }
        public BeforeStepAttribute(Type stepType)
        {
            Step = stepType;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class AfterStepAttribute : Attribute
    {
        public Type Step { get; }
        public AfterStepAttribute(Type stepType)
        {
            Step = stepType;
        }
    }
}
