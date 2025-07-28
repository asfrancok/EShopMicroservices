﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> _logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[START] Handle request={Request} - Response={Response} - Request Data={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var timeTaken = timer.Elapsed;
        if(timeTaken.Seconds > 3)
            _logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken}",
                typeof(TRequest).Name, timeTaken.Seconds);

        _logger.LogInformation("[END] Handled {Request} with {Response}",
            typeof(TRequest).Name, JsonSerializer.Serialize(response));

        return response;
    }
}