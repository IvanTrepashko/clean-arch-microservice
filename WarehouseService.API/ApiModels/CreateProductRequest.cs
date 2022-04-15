namespace WarehouseService.API.ApiModels
{
    public class CreateProductRequest
    {
        public string Name { get; set; }

        public int Stock { get; set; }

        public uint CategoryId { get; set; }
    }
}
