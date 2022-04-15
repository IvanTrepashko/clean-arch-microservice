using MediatR;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Application.Queries
{
    public class GetCategoryByIdQuery : IRequest<Category>
    {
        public uint Id { get; init; }
    }
}
