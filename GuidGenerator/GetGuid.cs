using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GuidGenerator;

public class GetGuid(ILoggerFactory loggerFactory)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<GetGuid>();

    // http://localhost:7269/api/GetGuid?count=10
    [Function("GetGuid")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        _logger.LogInformation("Started the GetGuid Function Call");

        string? numberOfGuidsText = req.Query["count"];
        List<string> guids = [];

        if (numberOfGuidsText != null && int.TryParse(numberOfGuidsText, out int numberOfGuids))
        {
            _logger.LogInformation("Number of Guids requested: {numberOfGuids}", numberOfGuids);
        }
        else
        {
            _logger.LogInformation("Unknown number of Guids requested. Using 1.");
            numberOfGuids = 1;
        }

        for (int i = 0; i < numberOfGuids; i++)
        {
            guids.Add(Guid.NewGuid().ToString());
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(guids);

        //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        //response.WriteString("Hello World");

        return response;
    }
}
