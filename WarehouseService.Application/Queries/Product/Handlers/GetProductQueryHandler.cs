using MediatR;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Application.Queries.Handlers
{
    internal class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
    {
        private readonly IProductRepository productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await productRepository.GetByIdAsync(request.Id);
        }
    }
}
