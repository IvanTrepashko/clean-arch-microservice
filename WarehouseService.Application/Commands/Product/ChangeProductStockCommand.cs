using MediatR;

namespace WarehouseService.Application.Commands
{
    public class ChangeProductStockCommand : IRequest
    {
        public uint Id { get; init; }

        public int Stock { get; init; }

        public ChangeProductStockCommand(uint id, int stock)
        {
            Id = id;
            Stock = stock;
        }
    }
}
