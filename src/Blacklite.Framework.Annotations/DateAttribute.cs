using System;
using System.ComponentModel.DataAnnotations;

namespace Blacklite.Framework.UI.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateAttribute : DataTypeAttribute
    {
        public DateAttribute()
            : base(DataType.Date)
        {
        }
    }
}
