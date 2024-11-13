using MongoDB.Driver;
using System.Threading.Tasks;

namespace minAPI
{
    //Anna
    public class LocationService
    {
        // MongoDB collection for PackageLocation
        private IMongoCollection<PackageLocation> _locationsCollection;

        //Constructor for connecting to MongoDB
        public LocationService()
        {
            // Connection string
            var connectionString = "mongodb://localhost:27017";
            var databaseName = "ParcelTrackingRUsDb";

            // Creates instance of MongoClient and retrieves database
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            //Accesses "locations" in the db 
            _locationsCollection = database.GetCollection<PackageLocation>("locations");
        }



        // Async method to add a location to the database
        public async Task AddLocationAsync(PackageLocation location)
        {
            await _locationsCollection.InsertOneAsync(location);
        }



        // Async method to get the latest location for a specific entity
        public async Task<PackageLocation> GetLatestLocationAsync(string entityId)
        {
            // Filter to find documents with the matching EntityID
            var filter = Builders<PackageLocation>.Filter.Eq("EntityID", entityId);

            // Sort the results by Time in descending order to get the most recent
            var sort = Builders<PackageLocation>.Sort.Descending("Time");

            // Find the first document that matches the filter
            var location = await _locationsCollection.Find(filter).Sort(sort).FirstOrDefaultAsync();

            return location;
        }
    }
}
