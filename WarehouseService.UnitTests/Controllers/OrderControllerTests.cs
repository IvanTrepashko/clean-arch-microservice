using AutoFixture.Xunit2;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using WarehouseService.API.ApiModels;
using WarehouseService.API.Controllers;
using WarehouseService.Application.Queries;
using WarehouseService.Domain.Entities;
using WarehouseService.Domain.Enums;
using WarehouseService.Infrastructure.Abstractions;
using WarehouseService.UnitTests.Setup;
using Xunit;

namespace WarehouseService.UnitTests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IIdGenerator> idGenerator;
        
        public OrderControllerTests()
        {
            idGenerator = new Mock<IIdGenerator>();
            idGenerator.Setup(x => x.GenerateIdAsync(It.IsAny<string>())).ReturnsAsync(1u);
        }

        [Theory, AutoMoqData]
        public void GetOrders_ReturnsListOfOrders([Frozen] Mock<ISender> sender)
        {
            // Arrange
            sender.Setup(x => x.Send(It.IsAny<GetOrdersQuery>(), CancellationToken.None))
                .ReturnsAsync(new GetOrdersResponse(ListOrders()));
            var orderController = new OrderController(sender.Object, idGenerator.Object);

            // Act
            var response = orderController.GetOrdersAsync().Result as JsonResult;
            var orders = response?.Value as List<Order>;

            // Assert.
            Assert.NotNull(response);
            Assert.NotNull(orders);
            Assert.Equal(StatusCodes.Status200OK, response?.StatusCode);
            Assert.Equal(ListOrders().Count, orders?.Count);
        }

        [Theory, AutoMoqData]
        public void GetOrder_OrderDoesNotExist_Returns_NotFound([Frozen] Mock<ISender> sender)
        {
            // Arrange
            sender.Setup(x => x.Send(It.IsAny<GetOrderQuery>(), CancellationToken.None))
                .ReturnsAsync(value: null);
            var orderController = new OrderController(sender.Object, idGenerator.Object);

            // Act
            var response = orderController.GetOrderAsync(1u).Result as NotFoundResult;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Theory, AutoMoqData]
        public void GetOrder_OrdedExists_ReturnsOrder(CreateOrderRequest createOrderRequest, [Frozen] Mock<ISender> sender)
        {
            // Arrange
            sender.Setup(x => x.Send(It.IsAny<GetOrderQuery>(), CancellationToken.None));
            var orderController = new OrderController(sender.Object, idGenerator.Object);

            // Act
            var response = orderController.CreateOrderAsync(createOrderRequest).Result as OkResult;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Theory, AutoMoqData]
        public void CreateOrder_ModelIsNotValid_ReturnsBadRequest(
            [NoAutoProperties] CreateOrderRequest request,
            [Frozen] Mock<ISender> sender)
        {
            // Arrange
            var orderController = new OrderController(sender.Object, idGenerator.Object);
            orderController.ModelState.AddModelError("test", "error");
            
            // Act
            var response = orderController.CreateOrderAsync(request).Result as BadRequestResult;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [Theory, AutoMoqData]
        public void CreateOrder_ModelIsValid_ReturnsOk(
            CreateOrderRequest request,
            [Frozen] Mock<ISender> sender)
        {
            // Arrange
            var orderController = new OrderController(sender.Object, idGenerator.Object);

            // Act
            var response = orderController.CreateOrderAsync(request).Result as OkResult;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }
        
        private static List<Order> ListOrders()
        {
            return new List<Order>()
            {
                new Order(1, 1, 1, 1, OrderStatus.Approved),
                new Order(2, 2, 2, 2, OrderStatus.Declined),
                new Order(3, 3, 3, 3, OrderStatus.Pending)
            };
        }
    }
}
