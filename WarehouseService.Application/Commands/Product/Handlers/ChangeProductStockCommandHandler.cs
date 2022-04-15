using MassTransit;
using MediatR;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Events;
using WarehouseService.Domain.Exceptions;

namespace WarehouseService.Application.Commands.Handlers
{
    internal class ChangeProductStockCommandHandler : IRequestHandler<ChangeProductStockCommand>
    {
        private readonly IProductRepository productRepository;
        private readonly IBus bus;

        public ChangeProductStockCommandHandler(IProductRepository productRepository, IBus bus)
        {
            this.productRepository = productRepository;
            this.bus = bus;
        }

        public async Task<Unit> Handle(ChangeProductStockCommand request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.Id);

            if (product is null)
            {
                throw new NotFoundException(request.Id);
            }

            product.Stock = request.Stock;
            await productRepository.UpdateAsync(product);

            await bus.Publish(new StockChanged()
            {
                ProductId = request.Id
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
