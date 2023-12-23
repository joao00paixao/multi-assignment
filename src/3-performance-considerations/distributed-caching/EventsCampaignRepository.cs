// For the sake of brevity let's pretend we have configured as K8S statefulset with a redis image and it is running on the same internal network so this application can access to redis.

// Redis is a key value dictionary

// Key: EventCampaigns Value: List of all event campaigns
// Key: Events Value: List of all events

// This is only an implementation for fetching values from the redis db. It does not have a refresh mechanism. We can implement said mechanism with a message queue and create a consumer in this same application, when a message is received we fetch all data from the database to update on the cache. We can implement this consumer with MassTransit easily.

public class EventCampaignsRepository : IRepository<EventCampaigns>{

    private readonly DbContext _dbContext;
    private readonly IDatabase _redisDb;

    public EventsCampaignRepository(DbContext dbContext)
    {
        _dbContext = dbContext;

        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
        IDatabase db = redis.GetDatabase(0);
        _redisDb = db;

        // Let's set some data in Redis manually... to apply this automatically we could use a BackgroundService, another application/consumer that does this automatically or via message.

        // We can also create a class representation of these objects and use a redis provider to directly insert the instantiated classes and query them.

        // For brevity we'll just use strings and JSON. We can then deserialize / serialize accordingly. We could also use a redis HashSet if we do not want to "get" the entire list of event campaigns on the "EventCampaigns" key.

        db.StringSet("EventCampaigns", "[
            {
                "id": 0
                "name": "Black Friday",  
                "startDate": "2023/11/15 00:00:00",  
                "endDate": "2023/11/22 23:59:59"  
            },
            {
                "id": 1
                "name": "Black Friday 2",  
                "startDate": "2023/11/15 00:00:00",  
                "endDate": "2023/11/22 23:59:59"  
            }
        ]");
    }

    public ICollection<EventCampaigns> GetAll()
    {
        var allEventCampaignsString = _redisDb.StringGet("EventCampaigns");

        var allEventCampaigns = JSON.Deserialize<ICollection<EventCampaign>>(allEventCampaignsString)

        // This collection of event campaigns could be saved on the application cache too, so we don't have to do multiple requests to redis. We can use Memcached for example.

        return allEventCampaigns;
    }
}