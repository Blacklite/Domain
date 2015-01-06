using Blacklite.Framework.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blacklite.Framework.Domain.Process
{
    public interface ISaveRequest<T, TResponse> : IRequest<TResponse>
        where TResponse : IProcessResponse<T>
        where T : class
    {
        T Request { get; }
    }

    public class SaveRequest<T, TResponse> : ISaveRequest<T, TResponse>
        where TResponse : IProcessResponse<T>
        where T : class
    {
        public SaveRequest(T instance)
        {
            Request = instance;
        }

        public T Request { get; }
    }

    public interface IProcessResponse<T> : IResponse
        where T : class
    {
        T Result { get; set; }
    }

    class ProcessResponse<T> : IProcessResponse<T>
        where T : class
    {
        public T Result { get; set; }
        public bool Success { get; set; }
    }

    public interface IInitRequest<T, TResponse> : IRequest<TResponse>
        where TResponse : IProcessResponse<T>
        where T : class
    { }

    abstract class InitRequest<T, TResponse> : IInitRequest<T, TResponse>
        where TResponse : IProcessResponse<T>
        where T : class
    { }

    public abstract class ProcessRequestHandler<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class, IResponse, new()
    {
        private readonly IEnumerable<IPreRequestHandler<TRequest>> _preRequestHandlers;
        private readonly IEnumerable<IPostRequestHandler<TRequest, TResponse>> _postRequestHandlers;
        protected IStepProvider StepProvider { get; }
        protected IProcessContextProvider ProcessContextProvider { get; }
        protected IServiceProvider ServiceProvider { get; }

        public ProcessRequestHandler(IEnumerable<IPreRequestHandler<TRequest>> preRequestHandlers,
            IEnumerable<IPostRequestHandler<TRequest, TResponse>> postRequestHandlers,
            IStepProvider stepProvider,
            IProcessContextProvider processContextProvider,
            IServiceProvider serviceProvider)
        {
            _preRequestHandlers = preRequestHandlers;
            _postRequestHandlers = postRequestHandlers;
            StepProvider = stepProvider;
            ProcessContextProvider = processContextProvider;
            ServiceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(TRequest message)
        {
            foreach (var preRequestHandler in _preRequestHandlers)
            {
                preRequestHandler.Handle(message);
            }

            var result = await Execute(message);

            foreach (var postRequestHandler in _postRequestHandlers)
            {
                postRequestHandler.Handle(message, result);
            }

            return result;
        }

        public abstract Task<TResponse> Execute(TRequest message);
    }

    public class ProcessSaveRequestHandler<T, TRequest, TResponse> : ProcessRequestHandler<TRequest, TResponse>
        where TRequest : ISaveRequest<T, TResponse>
        where TResponse : class, IProcessResponse<T>, new()
        where T : class
    {
        public ProcessSaveRequestHandler(
            IEnumerable<IPreRequestHandler<TRequest>> preRequestHandlers,
            IEnumerable<IPostRequestHandler<TRequest, TResponse>> postRequestHandlers,
            IStepProvider stepProvider,
            IProcessContextProvider processContextProvider,
            IServiceProvider serviceProvider)
            : base(preRequestHandlers, postRequestHandlers, stepProvider, processContextProvider, serviceProvider)
        {
        }

        public override Task<TResponse> Execute(TRequest message)
        {
            var response = new TResponse();

            var processContext = ProcessContextProvider.GetContextFor(message.Request);

            var steps = StepProvider.GetSaveSteps(message.Request, processContext);

            var validations = new List<IValidation>();
            foreach (var phase in steps)
            {
                foreach (var descriptor in phase)
                {
                    validations.AddRange(descriptor.Execute(message.Request, processContext));
                }

                if (validations.Any())
                {
                    response.Success = false;
                    response.Result = default(T);
                    return Task.FromResult(response);
                }
            }

            response.Success = !validations.Any();
            return Task.FromResult(response);
        }
    }

    //public class ProcessInitRequestHandler<T, TRequest, TResponse> : ProcessRequestHandler<TRequest, TResponse>
    //    where TRequest : IInitRequest<T, TResponse>
    //    where TResponse : class, IProcessResponse<T>, new()
    //    where T : class
    //{
    //    public override Task<TResponse> Execute(TRequest message)
    //    {
    //        var response = new TResponse();

    //        var processContext = ProcessContextProvider.GetContextFor(message.Request);

    //        var steps = StepProvider.GetInitSteps(message.Request, processContext);

    //        var validations = new List<IValidation>();
    //        foreach (var phase in steps)
    //        {
    //            foreach (var descriptor in phase)
    //            {
    //                validations.AddRange(descriptor.Execute(message.Request, processContext));
    //            }

    //            if (validations.Any())
    //            {
    //                response.Success = false;
    //                response.Result = default(T);
    //                return Task.FromResult(response);
    //            }
    //        }

    //        response.Success = !validations.Any();
    //        return Task.FromResult(response);
    //    }
    //}
}
