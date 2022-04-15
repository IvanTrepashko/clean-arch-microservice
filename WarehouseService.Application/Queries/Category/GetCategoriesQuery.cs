using MediatR;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Application.Queries
{
    public class GetCategoriesQuery : IRequest<CategoriesResponse>
    {
    }

    public class CategoriesResponse
    {
        public IEnumerable<Category> Categories { get; init; }

        public CategoriesResponse(IEnumerable<Category> categories)
        {
            Categories = categories;
        }
    }
}
