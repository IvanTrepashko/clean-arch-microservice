using MediatR;
using WarehouseService.Domain.Abstractions;

namespace WarehouseService.Application.Queries.Handlers
{
    internal class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsResponse>
    {
        private readonly IProductRepository productRepository;

        public GetProductsQueryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<GetProductsResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return new GetProductsResponse(await productRepository.GetAllAsync());
        }
    }
}
