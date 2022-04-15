namespace WarehouseService.Domain.Entities
{
    public class Category
    {
        public uint Id { get; init; }

        public string Name { get; init; }

        public int OutOfStockCount { get; init; }

        public int LowStockCount { get; init; }

        private Category()
        {
        }

        public Category(uint id, string name, int outOfStockCount, int lowStockCount)
        {
            if (outOfStockCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(outOfStockCount), $"{nameof(outOfStockCount)} cannot be less than 0.");
            }

            if (lowStockCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lowStockCount), $"{nameof(lowStockCount)} cannot be less than 0.");
            }

            if (outOfStockCount > lowStockCount)
            {
                throw new ArgumentOutOfRangeException(nameof(outOfStockCount), $"{nameof(outOfStockCount)} cannot be greater than {nameof(lowStockCount)}");
            }

            Id = id;
            Name = name;
            OutOfStockCount = outOfStockCount;
            LowStockCount = lowStockCount;
        }
    }
}
