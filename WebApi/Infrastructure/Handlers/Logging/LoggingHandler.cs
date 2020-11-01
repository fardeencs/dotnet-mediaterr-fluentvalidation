using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.Results;
using log4net;
using MediatR;
using WebApi.Infrastructure.Processes;
#pragma warning disable 693

namespace WebApi.Infrastructure.Handlers.Logging
{
    public class LoggingHandler<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>, IRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> inner;
        private readonly ILogger<TRequest, TResponse> logger;

        public LoggingHandler(IAsyncRequestHandler<TRequest, TResponse> inner, ILogger<TRequest, TResponse> logger)
        {
            this.inner = inner;
            this.logger = logger;
        }

        Task<TResponse> IAsyncRequestHandler<TRequest, TResponse>.Handle(TRequest message)
        {
            logger.LogRequestInfo<TRequest, TResponse>(message);
            var response = inner.Handle(message);
            logger.LogResponseInfo(response);
            return response;
        }
    }

    
}