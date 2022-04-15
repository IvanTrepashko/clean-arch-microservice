namespace WarehouseService.API.ApiModels
{
    public class ProductModel
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public int Stock { get; set; }

        public uint CategoryId { get; set; }
    }
}
