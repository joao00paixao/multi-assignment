// This is just an example class of a minimal API with a /customers/{id} endpoint that returns the customer ID.

public class Program{
    app.MapGet("/customers/{id}", (Guid id) => Results.Ok($"Customer ID: {id}"));
}