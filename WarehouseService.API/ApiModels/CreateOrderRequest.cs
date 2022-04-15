using WarehouseService.API.Validation;
using WarehouseService.Domain.Enums;

namespace WarehouseService.API.ApiModels
{
    public class CreateOrderRequest
    {
        [MinUIntValue(1)]
        public uint ProductId { get; set; }

        [MinUIntValue(1)]
        public uint UserId { get; set; }

        [MinIntValue(1)]
        public int Count { get; set; }

        public OutOfStockMode OutOfStockMode { get; set; }
    }
}
