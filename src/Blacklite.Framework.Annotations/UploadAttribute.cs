using System;
using System.ComponentModel.DataAnnotations;

namespace Blacklite.Framework.UI.Annotations
{
    public class UploadAttribute : DataTypeAttribute
    {
        public UploadAttribute()
            : base(DataType.Upload)
        {
        }
    }
}
