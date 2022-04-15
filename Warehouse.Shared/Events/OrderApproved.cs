namespace Warehouse.Shared.Events
{
    public class OrderApproved
    {
        public Guid EventId { get; set; }

        public uint OrderId { get; set; }

        public uint UserId { get; set; }

        public uint ProductId { get; set; }

        public int Count { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
