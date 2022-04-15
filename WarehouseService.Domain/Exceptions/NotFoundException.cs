namespace WarehouseService.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public uint ItemId { get; init; }

        public NotFoundException(uint itemId)
        {
            ItemId = itemId;
        }
    }
}
