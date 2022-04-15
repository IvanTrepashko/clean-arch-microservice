using MediatR;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Application.Queries.Handlers
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category>
    {
        private readonly ICategoryRepository categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await categoryRepository.GetByIdAsync(request.Id);
        }
    }
}
