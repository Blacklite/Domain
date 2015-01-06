using Blacklite.Framework.Domain.Requests;
using System;
using System.Collections.Generic;

namespace Blacklite.Framework.Domain.Process.Requests
{
    public interface IProcessResponse<T> : IResponse
        where T : class
    {
        T Result { get; set; }
        IEnumerable<IValidation> Errors { get; set; }
    }

    public class ProcessResponse<T> : IProcessResponse<T>
        where T : class
    {
        public T Result { get; set; }
        public IEnumerable<IValidation> Errors { get; set; }
        public bool Success { get; set; }
    }
}
