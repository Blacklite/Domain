using System;
using System.ComponentModel.DataAnnotations;

namespace Blacklite.Framework.UI.Annotations
{
    public class HtmlAttribute : DataTypeAttribute
    {
        public HtmlAttribute()
            : base(DataType.Html)
        {
        }
    }
}
