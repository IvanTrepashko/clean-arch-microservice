using MediatR;

namespace WarehouseService.Application.Commands
{
    public class CreateCategoryCommand : IRequest
    {
        public Domain.Entities.Category Category { get; init; }

        public CreateCategoryCommand(Domain.Entities.Category category)
        {
            Category = category;
        }
    }
}
