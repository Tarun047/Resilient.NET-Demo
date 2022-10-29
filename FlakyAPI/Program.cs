using System.Net;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/api/flaky-endpoint", () =>
{
    var shouldFail = Random.Shared.Next() % 2 == 0;
    if (shouldFail)
    {
        return Results.Problem(statusCode: (int)HttpStatusCode.ServiceUnavailable,
            detail: "That's a server unavailable");
    }

    return Results.Ok("Some valuable result");
});
app.Run();