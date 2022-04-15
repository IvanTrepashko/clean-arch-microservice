namespace WarehouseService.API.Options
{
    public class RabbitMqOptions
    {
        public string Host { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"amqp://{Username}:{Password}@{Host}";
            }
        }
    }
}
