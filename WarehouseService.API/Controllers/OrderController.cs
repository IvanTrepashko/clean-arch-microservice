using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WarehouseService.API.ApiModels;
using WarehouseService.Application.Commands;
using WarehouseService.Application.Queries;
using WarehouseService.Infrastructure.Abstractions;

namespace WarehouseService.API.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly ISender sender;
        private readonly IIdGenerator idGenerator;

        public OrderController(ISender sender, IIdGenerator idGenerator)
        {
            this.sender = sender;
            this.idGenerator = idGenerator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var createOrderCommand = new CreateOrderCommand(request.UserId, request.ProductId,
                request.Count, await idGenerator.GenerateIdAsync("Order"), request.OutOfStockMode);

            await sender.Send(createOrderCommand);

            return Ok();
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var getOrdersQuery = new GetOrdersQuery();

            var orders = await sender.Send(getOrdersQuery);

            return new JsonResult(orders.Orders) { StatusCode = StatusCodes.Status200OK};
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        public async Task<IActionResult> GetOrderAsync([FromRoute] uint id)
        {
            var getOrderQuery = new GetOrderQuery(id);

            var order = await sender.Send(getOrderQuery);

            if (order is null)
            {
                return NotFound();
            }

            return new JsonResult(order);
        }

        [HttpGet]
        [Route("by-user/{id}")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrdersByUserAsync([FromRoute] uint userId)
        {
            var getOrdersByUserQuery = new GetOrdersByUserIdQuery(userId);

            var orders = await sender.Send(getOrdersByUserQuery);

            return new JsonResult(orders.Orders);
        }
    }
}
