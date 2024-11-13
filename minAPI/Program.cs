using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace minAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Anna
            var builder = WebApplication.CreateBuilder(args); // Create a new webApplication builder

            builder.Services.AddSingleton<LocationService>(); // Add the LocationService to the dependency injection container

            var app = builder.Build(); // Build webApplication

            // Define the POST endpoint to add a new package location
            app.MapPost("/api/location", async (LocationService service, PackageLocation location) =>
            {
                location.Time = DateTime.UtcNow;

                // Add the location to the database
                await service.AddLocationAsync(location);
                return Results.Ok("Location added successfully.");
            });


            //Anna
            // Define the GET endpoint to retrieve the latest location for a specific entity
            app.MapGet("/api/location/{entityId}", async (LocationService service, string entityID) =>
            {
                // Get the latest location for the given entityID
                var location = await service.GetLatestLocationAsync(entityID);

              
                if (location != null)
                {
                    // Return the location data
                    return Results.Ok(location);
                }
                else
                {   
                    return Results.NotFound("Entity not found.");
                }
            });

            app.Run();
        }
    }
}
