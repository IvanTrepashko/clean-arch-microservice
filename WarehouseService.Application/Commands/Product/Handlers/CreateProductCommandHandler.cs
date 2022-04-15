using MediatR;
using WarehouseService.Domain.Abstractions;

namespace WarehouseService.Application.Commands.Handlers
{
    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(request.Product.CategoryId);
            
            if (category is null)
            {
                throw new ArgumentOutOfRangeException(nameof(request.Product.CategoryId), "Invalid category ID.");
            }

            await productRepository.AddAsync(request.Product);

            return Unit.Value;
        }
    }
}
