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
    public class ProductController : BaseApiController
    {
        private readonly IIdGenerator idGenerator;
        private readonly ISender sender;
        private readonly IMapper mapper;

        public ProductController(IIdGenerator idGenerator, ISender sender, IMapper mapper)
        {
            this.idGenerator = idGenerator;
            this.sender = sender;
            this.mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductRequest request)
        {
            var productModel = new ProductModel()
            {
                Id = await idGenerator.GenerateIdAsync("Product"),
                CategoryId = request.CategoryId,
                Name = request.Name,
                Stock = request.Stock
            };

            var product = mapper.Map<Product>(productModel);

            var createProductCommand = new CreateProductCommand(product);

            await sender.Send(createProductCommand);

            return Ok();
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts()
        {
            return new JsonResult((await sender.Send(new GetProductsQuery())).Products);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        public async Task<IActionResult> GetProduct([FromRoute] uint id)
        {
            var getProductQuery = new GetProductQuery(id);

            var product = await sender.Send(getProductQuery);

            if (product is null)
            {
                return NotFound();
            }

            return new JsonResult(product);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] uint id, [FromBody] ProductModel productModel)
        {
            var product = mapper.Map<Product>(productModel);

            var updateProductCommand = new UpdateProductCommand(product, id);

            await sender.Send(updateProductCommand);

            return Ok();
        }

        [HttpPut]
        [Route("change-stock/{id}")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        public async Task<IActionResult> ChangeStockAsync([FromRoute] uint id, [FromBody] int stock)
        {
            if (stock < 0)
            {
                return BadRequest();
            }

            var changeProductStockCommand = new ChangeProductStockCommand(id, stock);

            await sender.Send(changeProductStockCommand);

            return Ok();
        }
    }
}
