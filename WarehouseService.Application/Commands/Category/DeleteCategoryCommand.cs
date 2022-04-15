using MediatR;

namespace WarehouseService.Application.Commands
{
    public class DeleteCategoryCommand : IRequest
    {
        public uint Id { get; init; }

        public DeleteCategoryCommand(uint id)
        {
            Id = id;
        }
    }
}
