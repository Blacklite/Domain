using System;
using System.ComponentModel.DataAnnotations;

namespace Blacklite.Framework.UI.Annotations
{
    public class TextAttribute : DataTypeAttribute
    {
        public TextAttribute()
            : base(DataType.Text)
        {
        }
    }
}
