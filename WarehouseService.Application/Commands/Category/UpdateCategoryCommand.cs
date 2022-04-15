using MediatR;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Application.Commands
{
    public class UpdateCategoryCommand : IRequest
    {
        public uint Id { get; init; }

        public Category Category { get; init; }

        public UpdateCategoryCommand(uint id, Category category)
        {
            Id = id;
            Category = category;
        }
    }
}
