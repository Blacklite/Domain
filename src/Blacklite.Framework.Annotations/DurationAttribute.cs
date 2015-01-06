using System;
using System.ComponentModel.DataAnnotations;

namespace Blacklite.Framework.UI.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DurationAttribute : DataTypeAttribute
    {
        public DurationAttribute()
            : base(DataType.Duration)
        {
        }
    }
}
