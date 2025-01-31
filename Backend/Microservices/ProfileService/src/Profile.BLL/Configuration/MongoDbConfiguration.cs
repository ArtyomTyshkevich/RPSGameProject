
namespace Profile.BLL.Configuretion
{
    public class MongoDbConfiguration
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string GameCollectionName { get; set; } = null!;
    }
}
