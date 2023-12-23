// Class Definitions for the example
public class MicroservicesInformation{
    public string Name { get; set;}
    public string ClusterIp { get; set;}
    public string IngressAddress { get; set;}
}

public class ApplicationConfiguration{
    public ICollection<MicroservicesInformation> MicroservicesInformation {get; set;}
}

// For the sake of brevity - let's pretend we binded and injected the correct appSettings file (either Development or Production, based on the Hosting Environment or an Environment Variable) representation as the ApplicationConfiguration and an HttpClient.

// If we're planning to have multiple requests we should implement an HttpClientFactory (or reuse the HttpClientHandler) so we can avoid port exhaustion.

// And these injections should depend on Interfaces and not actual classes, but let's keep it simple for the example.

public class Example{
    private ApplicationConfiguration _appConfiguration;
    private HttpClient _httpClient;

    public HttpClient(ApplicationConfiguration injectedAppConfiguration, HttpClient injectedHttpClient)
    {
        _appConfiguration = injectedAppConfiguration;
        _httpClient = injectedHttpClient;
    }

    public async string GetCustomer(Guid customerId){
        var microserviceAAddress = _appConfiguration.MicroservicesInformation.First(d => d.Name == "microservice-a").IngressAddress; 
        
        // If the microservice A is internal or lives in the same cluster as this application we can use either the ingress address or the Cluster IP. If it's external (living in another kubernetes cluster) or this application is outside of the cluster we must use the Ingress Address.

        var response = await _httpClient.GetAsync(microserviceAAddress + "/customers/" + customerId);

        // Continue flow - the result would be fetched if this application has the network access necessary. The .First() could return an exception if the microservice-a doesn't exist in the application configuration.
    }
}