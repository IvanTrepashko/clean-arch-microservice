using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WarehouseService.API.ApiModels;
using WarehouseService.Application.Commands;
using WarehouseService.Application.Queries;
using WarehouseService.Domain.Entities;
using WarehouseService.Infrastructure.Abstractions;

namespace WarehouseService.API.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly IMapper mapper;
        private readonly ISender sender;
        private readonly IIdGenerator generator;

        public CategoryController(IMapper mapper, ISender sender, IIdGenerator generator)
        {
            this.mapper = mapper;
            this.sender = sender;
            this.generator = generator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if (request.LowStockCount <= request.OutOfStockCount)
            {
                return BadRequest("LowStockCount cannot less than OutOfStockCount");
            }


            var categoryModel = new CategoryModel()
            {
                Id = await generator.GenerateIdAsync("Category"),
                Name = request.Name,
                LowStockCount = request.LowStockCount,
                OutOfStockCount = request.OutOfStockCount
            };

            var category = mapper.Map<Category>(categoryModel);

            var createCategoryCommand = new CreateCategoryCommand(category);

            await sender.Send(createCategoryCommand);

            return Ok();
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategories()
        {
            var getCategoriesQuery = new GetCategoriesQuery();

            var categories = await sender.Send(getCategoriesQuery);

            return new JsonResult(categories.Categories);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCategory([FromRoute] uint id)
        {
            var deleteCategoryCommand = new DeleteCategoryCommand(id);

            await sender.Send(deleteCategoryCommand);

            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        public async Task<IActionResult> GetCategory([FromRoute] uint id)
        {
            var getCategoryByIdQuery = new GetCategoryByIdQuery()
            {
                Id = id
            };

            var category = await sender.Send(getCategoryByIdQuery);

            if (category is null)
            {
                return NotFound();
            }

            return new JsonResult(category);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCategory([FromRoute] uint id, [FromBody] CategoryModel categoryModel)
        {
            var category = mapper.Map<Category>(categoryModel);

            var updateCategoryCommand = new UpdateCategoryCommand(id, category);

            await sender.Send(updateCategoryCommand);

            return Ok();
        }
    }
}
