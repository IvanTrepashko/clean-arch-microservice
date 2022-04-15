namespace WarehouseService.Domain.Entities
{
    public class Product
    {
        private int stock;

        public uint Id { get; init; }

        public string Name { get; init; }

        public int Stock
        {
            get
            {
                return stock;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Stock cannot be less than 0");
                }

                stock = value;
            }
        }

        public uint CategoryId { get; init; }

        public Product(uint id, string name, int stock, uint categoryId)
        {
            if (stock < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stock), "Stock cannot be less than 0");
            }

            Id = id;
            Name = name;
            Stock = stock;
            CategoryId = categoryId;
        }
    }
}
