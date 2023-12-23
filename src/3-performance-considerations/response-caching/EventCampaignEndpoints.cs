// Sample code for response caching
// Let's add a middleware that alters response cache control header for a max-age of 100 seconds. This can be configurable if we chain a .WithMetadata() method to the .MapGet() methods.

public class Program {
    app.UseMiddleware<AddCacheHeadersMiddleware>();

    app.AddEventCampaignEndpoints();
}

public class AddCacheHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public AddCacheHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext.GetEndpoint()?.Metadata.GetMetadata<CacheResponseMetadata>() is { } mutateResponseMetadata)
        {
            if (httpContext.Response.HasStarted)
            {
                throw new InvalidOperationException("Can't mutate response after headers have been sent to client.");
            }
            httpContext.Response.Headers.CacheControl = new[] { "public", "max-age=100" };
        }
        await _next(httpContext);
    }
}

public class Endpoints {
    public static void AddEventCampaignEndpoints(this WebApplication app)
    {
        app.MapGroup("/event-campaigns")
            .MapGet("/{id}", (int id) {
                return GetEventCampaign(id);
            })
            .MapGet("/{id}/events", (int id){
                return GetEventCampaignEvents(id);
            })
            .MapGet("/live", () {
                return GetLiveEventCampaigns();
            })
            .MapDelete("/{id}", (int id){
                return DeleteEventCampaign(id);
            })
            .MapPost("/", (EventCampaignRequest eventCampaign) {
                return CreateEventCampaign(eventCampaign);
            });

        app.MapGroup("/events")
            .MapGet("/{id}", (int id) {
                return GetEvent(int);
            })
            .MapGet("/live", () {
                return GetLiveEvents();
            })
            .MapDelete("/{id}", (int id){
                return DeleteEvent(id);
            })
            .MapPost("/", (EventRequest eventCampaign) {
                return CreateEvent(eventCampaign);
            });
    }



}

