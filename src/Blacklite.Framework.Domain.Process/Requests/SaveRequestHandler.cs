using Blacklite.Framework.Domain.Process.Steps;
using Blacklite.Framework.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blacklite.Framework.Domain.Process.Requests
{
    public interface ISaveRequestHandler<T> : IProcessRequestHandler<T, ISaveRequest<T>, IProcessResponse<T>>
        where T : class
    { }

    public class SaveRequestHandler<T> : ProcessRequestHandler<T, ISaveRequest<T>, IProcessResponse<T>>, ISaveRequestHandler<T>
        where T : class
    {
        public SaveRequestHandler(
            IEnumerable<IPreRequestHandler<ISaveRequest<T>>> preRequestHandlers,
            IEnumerable<IPostRequestHandler<ISaveRequest<T>, IProcessResponse<T>>> postRequestHandlers,
            IStepProvider stepProvider,
            IProcessContextProvider processContextProvider,
            IServiceProvider serviceProvider,
            IProcessResponse<T> response) : base(preRequestHandlers, postRequestHandlers, stepProvider, processContextProvider, serviceProvider, response)
        {
        }

        public override Task<IProcessResponse<T>> Execute(ISaveRequest<T> message)
        {
            var processContext = ProcessContextProvider.GetContextFor(message.Request);

            var steps = StepProvider.GetSaveSteps(message.Request, processContext);

            var validations = new List<IValidation>();
            Response.Errors = validations;
            foreach (var phase in steps)
            {
                foreach (var descriptor in phase)
                {
                    validations.AddRange(descriptor.Execute(message.Request, processContext));
                }

                if (validations.Any())
                {
                    Response.Success = false;
                    Response.Result = default(T);
                    return Task.FromResult(Response);
                }
            }

            Response.Result = message.Request;
            Response.Success = !validations.Any();
            return Task.FromResult(Response);
        }
    }
}
