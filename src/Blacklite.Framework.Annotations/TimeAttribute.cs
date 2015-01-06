using System;
using System.ComponentModel.DataAnnotations;

namespace Blacklite.Framework.UI.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TimeAttribute : DataTypeAttribute
    {
        public TimeAttribute()
            : base(DataType.Time)
        {
        }
    }
}
