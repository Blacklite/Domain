using System;
using System.ComponentModel.DataAnnotations;

namespace Blacklite.Framework.UI.Annotations
{
    public class PasswordAttribute : DataTypeAttribute
    {
        public PasswordAttribute()
            : base(DataType.Password)
        {
        }
    }
}
