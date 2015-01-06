using System;
using System.ComponentModel.DataAnnotations;

namespace Blacklite.Framework.UI.Annotations
{
    public class MultilineTextAttribute : DataTypeAttribute
    {
        public MultilineTextAttribute()
            : base(DataType.MultilineText)
        {
        }
    }
}
