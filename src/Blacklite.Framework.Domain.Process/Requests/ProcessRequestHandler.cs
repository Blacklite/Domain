using Blacklite.Framework.Domain.Process.Steps;
using Blacklite.Framework.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blacklite.Framework.Domain.Process.Requests
{
    public interface IProcessRequestHandler<T, TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    { }

    public abstract class ProcessRequestHandler<T, TRequest, TResponse> : IProcessRequestHandler<T, TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IProcessResponse<T>
        where T : class
    {
        private readonly IEnumerable<IPreRequestHandler<TRequest>> _preRequestHandlers;
        private readonly IEnumerable<IPostRequestHandler<TRequest, TResponse>> _postRequestHandlers;
        protected IStepProvider StepProvider { get; }
        protected IProcessContextProvider ProcessContextProvider { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected TResponse Response { get; }

        public ProcessRequestHandler(IEnumerable<IPreRequestHandler<TRequest>> preRequestHandlers,
            IEnumerable<IPostRequestHandler<TRequest, TResponse>> postRequestHandlers,
            IStepProvider stepProvider,
            IProcessContextProvider processContextProvider,
            IServiceProvider serviceProvider,
            TResponse response)
        {
            _preRequestHandlers = preRequestHandlers;
            _postRequestHandlers = postRequestHandlers;
            StepProvider = stepProvider;
            ProcessContextProvider = processContextProvider;
            ServiceProvider = serviceProvider;
            Response = response;
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

}
