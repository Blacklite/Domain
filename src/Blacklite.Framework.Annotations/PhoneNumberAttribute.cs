using System;
using System.ComponentModel.DataAnnotations;

namespace Blacklite.Framework.UI.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PhoneNumberAttribute : DataTypeAttribute
    {
        public PhoneNumberAttribute()
            : base(DataType.PhoneNumber)
        {
        }
    }
}
