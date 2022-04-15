using MediatR;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Exceptions;

namespace WarehouseService.Application.Commands.Handlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = categoryRepository.GetByIdAsync(request.Id);
            
            if (category is null)
            {
                throw new NotFoundException(request.Id);
            }

            var productsCount = (await productRepository.GetProductsByCategory(request.Id)).Count();

            if (productsCount == 0)
            {
                await categoryRepository.DeleteAsync(request.Id);
            }

            return Unit.Value;
        }
    }
}
