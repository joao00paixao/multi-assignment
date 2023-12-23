// For the sake of example brevity we'll use a Minimal API approach that returns a simple response.

public class Program
{
    app.AddEventCampaignEndpoints();
}

[Table("EventCampaigns", Schema = "Marketing")]
public class EventCampaign {
    [Key]
    public int Id { get; set; }
    public string Name {get; set;}
    public DateTime StartDate { get; set;}
    public DateTime EndDate {get; set;}
    public DateTime CreatedOn {get; set;}

    public virtual ICollection<Events> Events {get; set;}
}

[Table("Events", Schema = "Marketing")]
public class Event {
    [Key]
    public int Id { get; set; }
    public int EventCampaignId {get; set;}
    public string Name {get; set;}
    public DateTime StartDate { get; set;}
    public DateTime EndDate {get; set;}
    public DateTime CreatedOn {get; set;}

    [ForeignKey("EventCampaignId")]
    public virtual EventCampaign EventCampaign {get; set;}
}

public record EventCampaignRequest(string Name, DateTime StartDate, DateTime EndDate);
public record EventCampaignResponse(int Id, string Name, DateTime StartDate, DateTime EndDate, DateTime CreatedOn);

public record EventRequest(int EventCampaignId, string Name, DateTime StartDate, DateTime EndDate);
public record EventResponse(int Id, int EventCampaignId, string Name, DateTime StartDate, DateTime EndDate, DateTime CreatedOn);

public static class EventCampaignEndpoints 
{

    private readonly IEventCampaignRepository _eventCampaignRepository;
    private readonly IEventRepository _eventRepository;

    public EventCampaignEndpoints(IEventCampaignRepository injectedEventCampaignRepository, IEventRepository injectedEventRepository)
    {
        _eventCampaignRepository = injectedEventCampaignRepository;
        _eventRepository = injectedEventRepository;
    }

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

    // Returns an event campaign from: GET /event-campaigns/{id}
    public static Result<EventCampaign> GetEventCampaign(int campaignEventId)
    {
        return Ok(_eventCampaignRepository.GetEventCampaign(campaignEventId));
    }

    // Returns all sub-events of one campaign event from: GET /event-campaigns/{id}/events
    public static Result<ICollection<Event>> GetEventCampaignEvents(int campaignEventId)
    {
        var campaignEvent = _eventCampaignRepository.GetEventCampaign(campaignEventId);

        return Ok(campaignEvent.Events)
    }

    // Get all active event campaigns as of the moment from: GET /event-campaigns/live
    public static Result<ICollection<EventCampaign>> GetLiveEventCampaigns()
    {
        var activeEventCampaigns = _eventCampaignRepository.GetLiveEventCampaigns();

        return Ok(activeEventCampaigns);
    }

    // Delete event campaign from: DELETE /event-campaigns/{id}
    public static Result DeleteEventCampaign(int id){
        var result = _eventCampaignRepository.DeleteEventCampaign(id);

        return result;
    }
    
    // Create event campaign from: POST /event-campaigns/
    public static Result<EventCampaignResponse> CreateEventCampaign(EventCampaignRequest eventCampaign){
       var result =  _eventCampaignRepository.Add(eventCampaign);

        return result;
    }






    // Get event from: /events/{id}
    public static Result<Event> GetEvent(int id){
        var marketingEvent = _eventRepository.GetEvent(id);

        return Ok(marketingEvent);
    }

    // Get all live events from: /events/live
    public static Result<ICollection<Event>> GetLiveEvents(){
        var liveEvents = _eventRepository.GetLiveEvents();

        return Ok(liveEvents);
    }

    // Delete event from: DELETE /events/{id}
    public static Result DeleteEvent(int id){
        var result = _eventRepository.DeleteEvent(id);

        return result;
    }

    // Create event from: POST /events/
    public static Result<EventResponse> CreateEvent(EventRequest marketingEvent){
       var result =  _eventRepository.Add(marketingEvent);

        return result;
    }
}