using System;
using System.ComponentModel.DataAnnotations;

namespace Blacklite.Framework.UI.Annotations
{
    public class PostalCodeAttribute : DataTypeAttribute
    {
        public PostalCodeAttribute()
            : base(DataType.PostalCode)
        {
        }
    }
}
