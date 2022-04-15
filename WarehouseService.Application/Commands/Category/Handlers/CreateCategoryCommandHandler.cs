using MediatR;
using WarehouseService.Domain.Abstractions;

namespace WarehouseService.Application.Commands.Handlers
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
    {
        private readonly ICategoryRepository categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            await categoryRepository.AddAsync(request.Category);

            return Unit.Value;
        }
    }
}
