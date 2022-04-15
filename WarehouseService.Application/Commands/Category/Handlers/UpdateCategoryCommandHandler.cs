using MediatR;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Exceptions;

namespace WarehouseService.Application.Commands.Handlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(request.Id);

            if (category is null)
            {
                throw new NotFoundException(request.Id);
            }

            await categoryRepository.UpdateAsync(request.Category);

            return Unit.Value;
        }
    }
}
