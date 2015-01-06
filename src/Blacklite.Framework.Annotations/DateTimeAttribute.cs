using System;
using System.ComponentModel.DataAnnotations;

namespace Blacklite.Framework.UI.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateTimeAttribute : DataTypeAttribute
    {
        public DateTimeAttribute()
            : base(DataType.DateTime)
        {
        }
    }
}
