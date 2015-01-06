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
            IProcessResponse<T> response)
            : base(preRequestHandlers, postRequestHandlers, stepProvider, processContextProvider, serviceProvider, response)
        {
        }

        public override Task<IProcessResponse<T>> Execute(ISaveRequest<T> message)
        {
            // Go find the process context, this provider could return a derived context
            // that may contain other details such as the url, id, query string, etc.
            //  (totally up to the implementor)
            var processContext = ProcessContextProvider.GetContextFor(message.Request);

            // Get the steps for the stage
            var steps = StepProvider.GetSaveSteps(message.Request, processContext);

            var validations = new List<IValidation>();
            Response.Errors = validations;

            // Each phase runs sequentially
            foreach (var phase in steps)
            {
                foreach (var descriptor in phase)
                {
                    validations.AddRange(descriptor.Execute(message.Request, processContext));
                }

                // If a phase encounters errors, the request is aborted, and the errors returned.
                if (validations.Any())
                {
                    Response.Result = default(T);
                    break;
                }
            }

            // Success is decied by if there were any errors or not.
            Response.Success = !validations.Any();

            if (Response.Success)
                Response.Result = message.Request;

            return Task.FromResult(Response);
        }
    }
}
