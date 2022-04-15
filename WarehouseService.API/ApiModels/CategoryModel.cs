namespace WarehouseService.API.ApiModels
{
    public class CategoryModel
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public int OutOfStockCount { get; set; }

        public int LowStockCount { get; set; }
    }
}
