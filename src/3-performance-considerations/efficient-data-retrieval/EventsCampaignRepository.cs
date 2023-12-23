//DbContext has access to our database and the two tables: EventCampaigns and Events

public class EventCampaignsRepository : IRepository<EventCampaigns>{

    private readonly DbContext _dbContext;

    public EventsCampaignRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ICollection<EventCampaigns> GetAll()
    {
        return GetQueryableAsNoTracking().ToList(); 
        
        // This optimizes the application memory as it doesn't track changes made to the fetched objects. This is only required in case you update an object's properties and you Save the changes. If you don't update anything it's better to get a AsNoTracking() queryable.
    }

    public ICollection<EventCampaigns> GetAllWithSubEvents()
    {
        return GetQueryableAsNoTracking().IncludeFilter(d => d.Events(da => da.StartDate.Date => DateTime.Now.Date)).ToList(); 

        // instead of just using a normal .Include() we can use a .IncludeFilter() to do a JOIN with conditions. This is also a way to improve data retrieval.

        // and instead of accessing the DB to get each event for each event campaign... which is very bad :)
    }

    private IQueryable<EventCampaigns> GetQueryable(){
        return _dbContext.EventCampaigns.AsQueryable();
    }

    private IQueryable<EventCampaigns> GetQueryableAsNoTracking(){
        return _dbContext.EventCampaigns.AsNoTracking().AsQueryable();
    }
}