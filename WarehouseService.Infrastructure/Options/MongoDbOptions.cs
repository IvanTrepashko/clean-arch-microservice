namespace WarehouseService.Infrastructure.Options
{
    public class MongoDbOptions
    {
        public string Host { get; set; }

        public string DatabaseName { get; set; }

        public string MongoDbUsername { get; set; }

        public string MongoDbPassword { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"mongodb://{MongoDbUsername}:{MongoDbPassword}@{Host}";
            }
        }
    }
}
