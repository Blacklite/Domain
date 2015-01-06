using System;
using System.ComponentModel.DataAnnotations;

namespace Blacklite.Framework.UI.Annotations
{
    public class CurrencyAttribute : DataTypeAttribute
    {
        public CurrencyAttribute()
            : base(DataType.Currency)
        {
        }
    }
}
