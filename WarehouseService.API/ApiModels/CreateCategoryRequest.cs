namespace WarehouseService.API.ApiModels
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }

        public int OutOfStockCount { get; set; }

        public int LowStockCount { get; set; }

        public CreateCategoryRequest(string name, int outOfStockCount, int lowStockCount)
        {
            Name = name;
            OutOfStockCount = outOfStockCount;
            LowStockCount = lowStockCount;
        }
    }
}
