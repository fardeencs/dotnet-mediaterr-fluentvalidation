using System;
using System.Collections.Generic;
using FluentValidation.Results;
using MediatR;
#pragma warning disable 693

namespace WebApi.Infrastructure.Processes
{
    public interface ILogger<in TRequest, in TResponse> where TRequest : IAsyncRequest<TResponse>
    {
        void LogInfo(TRequest request, TResponse response);

        void LogError(TRequest request, TResponse response, Exception ex);

        void LogFluentValidationError(TRequest request, List<ValidationFailure> validationFailures);

        void LogRequestInfo<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>;

        void LogResponseInfo<TResponse>(TResponse response);

        void LogSql(string sql);
    }
}