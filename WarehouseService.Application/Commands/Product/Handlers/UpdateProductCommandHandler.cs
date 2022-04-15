using MassTransit;
using MediatR;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Events;
using WarehouseService.Domain.Exceptions;

namespace WarehouseService.Application.Commands.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository productRepository;
        private readonly IBus bus;

        public UpdateProductCommandHandler(IProductRepository productRepository, IBus bus)
        {
            this.productRepository = productRepository;
            this.bus = bus;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.Id);

            if (product is null)
            {
                throw new NotFoundException(request.Id);
            }
            
            await productRepository.UpdateAsync(request.Product);
                
            if (product.Stock != request.Product.Stock)
            {
                await bus.Publish(new StockChanged()
                {
                    ProductId = request.Id
                }, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
