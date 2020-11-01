using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Results;
using log4net;
using MediatR;
using WebApi.Infrastructure.Processes;
#pragma warning disable 693

namespace WebApi.Infrastructure.Handlers.Logging
{
    public class Logger<TRequest, TResponse> : ILogger<TRequest, TResponse> where TRequest : IAsyncRequest<TResponse>
    {
        private readonly ILog _log;

        public Logger(ILog log)
        {
            this._log = log;
        }

        public void LogInfo(TRequest request, TResponse response)
        {
            Log(request, response);
        }

        public void LogError(TRequest request, TResponse response, Exception ex)
        {
            Log(request, response, ex);
        }

        public void LogFluentValidationError(TRequest request, List<ValidationFailure> validationFailures)
        {
            _log.Info("Log with exception for" + DateTime.Now);
            _log.Debug(new RequestResponseLogMessage<TRequest, TResponse>
            {
                Request = request,
                ValidationFailures = validationFailures
            });
        }

        public void LogRequestInfo<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>
        {
            _log.Info("Log for Response" + DateTime.Now);
            _log.Debug(new RequestResponseLogMessage<TRequest, TResponse>
            {
                Request = request
            });
        }

        public void LogResponseInfo<TResponse>(TResponse response)
        {
            _log.Info("Log for Response" + DateTime.Now);
            _log.Debug(new RequestResponseLogMessage<TRequest, TResponse>
            {
                Response = response
            });
        }

        public void LogSql(string sql)
        {
            _log.Info("Sql Log");
            _log.Debug(sql);
        }

        private void Log(TRequest request, TResponse response)
        {
            Log(request, response, null);
        }

        private void Log(TRequest request, TResponse response, Exception ex)
        {
            if (ex == null)
            {
                _log.Info("Log for" + DateTime.Now);
                _log.Debug(new RequestResponseLogMessage<TRequest, TResponse>
                {
                    Request = request,
                    Response = response
                });
            }
            else
            {
                _log.Info("Log with exception for" + DateTime.Now);
                _log.Debug(new RequestResponseLogMessage<TRequest, TResponse>
                {
                    Request = request,
                    Response = response,
                    Exception = ex
                });
            }
        }
    }

    internal class RequestResponseLogMessage<TRequest, TResponse>
    {
        public TRequest Request { get; set; }
        public TResponse Response { get; set; }
        public Exception Exception { get; set; }
        public List<ValidationFailure> ValidationFailures { get; set; }
    }
}