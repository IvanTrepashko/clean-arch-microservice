using MediatR;
using WarehouseService.Domain.Abstractions;

namespace WarehouseService.Application.Queries.Handlers
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, CategoriesResponse>
    {
        private readonly ICategoryRepository categoryRepository;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        async Task<CategoriesResponse> IRequestHandler<GetCategoriesQuery, CategoriesResponse>.Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await categoryRepository.GetAllAsync();

            return new CategoriesResponse(categories);
        }
    }
}
