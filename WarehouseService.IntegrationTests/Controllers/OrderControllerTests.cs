using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using WarehouseService.API.ApiModels;
using WarehouseService.Domain.Entities;
using WarehouseService.Domain.Enums;
using WarehouseService.IntegrationTests.Setup;
using Xunit;

namespace WarehouseService.IntegrationTests.Controllers
{
    [CollectionDefinition("OrderController", DisableParallelization = true)]
    public class OrderControllerTests : IClassFixture<TestWebApplicationFactory<API.Program>>
    {
        private readonly HttpClient client;
        private readonly TestWebApplicationFactory<API.Program> factory;

        public OrderControllerTests(TestWebApplicationFactory<API.Program> factory)
        {
            this.factory = factory;
            this.client = this.factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory, AutoMoqData]
        public async void CreateOrder_CountIsMoreThanStock_OrderIsPending(
            CreateOrderRequest orderRequest,
            CreateCategoryRequest categoryRequest,
            CreateProductRequest productRequest)
        {
            // Arrange
            categoryRequest.OutOfStockCount = 0;
            categoryRequest.LowStockCount = 3;
            
            await client.PostAsJsonAsync("api/category/", categoryRequest);

            var category = client.GetFromJsonAsync<IEnumerable<Category>>("api/category/all").Result
                .FirstOrDefault(x => x.OutOfStockCount == 0 && x.LowStockCount == 3);

            productRequest.CategoryId = category.Id;

            productRequest.Stock = 2;

            await client.PostAsJsonAsync("api/product", productRequest);

            var product = client.GetFromJsonAsync<IEnumerable<Product>>("api/product/all").Result
                .FirstOrDefault(x => x.CategoryId == productRequest.CategoryId && x.Stock == productRequest.Stock);

            orderRequest.Count = 4;
            orderRequest.OutOfStockMode = OutOfStockMode.ReserveWhenAvailable;
            orderRequest.ProductId = product.Id;
            orderRequest.UserId = 1;

            // Act
            await client.PostAsJsonAsync("api/order", orderRequest);
            var createdOrder = client.GetFromJsonAsync<IEnumerable<Order>>("api/order/all").Result
                .FirstOrDefault(x => x.UserId == 1 && x.Count == 4 && x.OrderStatus == OrderStatus.Pending && x.ProductId == product.Id);

            // Assert
            Assert.NotNull(createdOrder);
            Assert.Equal(createdOrder.OrderStatus, OrderStatus.Pending);
        }

        [Theory, AutoMoqData]
        public async void CreateOrder_ManualApproval_OrderApproved(
            CreateOrderRequest orderRequest,
            CreateCategoryRequest categoryRequest,
            CreateProductRequest productRequest)
        {
            // Arrange
            categoryRequest.OutOfStockCount = 0;
            categoryRequest.LowStockCount = 10;

            await client.PostAsJsonAsync("api/category/", categoryRequest);

            var category = client.GetFromJsonAsync<IEnumerable<Category>>("api/category/all").Result
                .FirstOrDefault(x => x.OutOfStockCount == 0 && x.LowStockCount == 10);

            productRequest.CategoryId = category.Id;

            productRequest.Stock = 8;

            await client.PostAsJsonAsync("api/product", productRequest);

            var product = client.GetFromJsonAsync<IEnumerable<Product>>("api/product/all").Result
                .FirstOrDefault(x => x.CategoryId == productRequest.CategoryId && x.Stock == productRequest.Stock);

            orderRequest.Count = 6;
            orderRequest.OutOfStockMode = OutOfStockMode.ReserveWhenAvailable;
            orderRequest.ProductId = product.Id;
            orderRequest.UserId = 2;

            // Act
            await client.PostAsJsonAsync("api/order", orderRequest);

            Thread.Sleep(1_000);

            var createdOrder = client.GetFromJsonAsync<IEnumerable<Order>>("api/order/all").Result
                .FirstOrDefault(x => x.UserId == 2 && x.Count == 6 && x.ProductId == product.Id && x.OrderStatus == OrderStatus.Approved);

            // Assert
            Assert.NotNull(createdOrder);
            Assert.Equal(OrderStatus.Approved, createdOrder.OrderStatus);
            Assert.Equal(6, createdOrder.Count);
            Assert.Equal(2u, createdOrder.UserId);
        }

        [Theory, AutoMoqData]
        public async void CreateOrder_ManualApproval_OrderDeclined(
           CreateOrderRequest orderRequest,
           CreateCategoryRequest categoryRequest,
           CreateProductRequest productRequest)
        {
            // Arrange
            categoryRequest.OutOfStockCount = 0;
            categoryRequest.LowStockCount = 10;

            await client.PostAsJsonAsync("api/category/", categoryRequest);

            var category = client.GetFromJsonAsync<IEnumerable<Category>>("api/category/all").Result
                .FirstOrDefault(x => x.OutOfStockCount == 0 && x.LowStockCount == 10);

            productRequest.CategoryId = category.Id;

            productRequest.Stock = 8;

            await client.PostAsJsonAsync("api/product", productRequest);

            var product = client.GetFromJsonAsync<IEnumerable<Product>>("api/product/all").Result
                .FirstOrDefault(x => x.CategoryId == productRequest.CategoryId && x.Stock == productRequest.Stock);

            orderRequest.Count = 7;
            orderRequest.OutOfStockMode = OutOfStockMode.ReserveWhenAvailable;
            orderRequest.ProductId = product.Id;
            orderRequest.UserId = 3;

            // Act
            await client.PostAsJsonAsync("api/order", orderRequest);

            Thread.Sleep(1_000);

            var createdOrder = client.GetFromJsonAsync<IEnumerable<Order>>("api/order/all").Result
                .FirstOrDefault(x => x.UserId == 3 && x.Count == 7 && x.ProductId == product.Id);

            // Assert
            Assert.NotNull(createdOrder);
            Assert.Equal(OrderStatus.Declined, createdOrder.OrderStatus);
            Assert.Equal(7, createdOrder.Count);
            Assert.Equal(3u, createdOrder.UserId);
        }

        [Theory, AutoMoqData]
        public async void CreateOrder_OrderCreatedAfterStockUpdate(
            CreateOrderRequest orderRequest,
            CreateCategoryRequest categoryRequest,
            CreateProductRequest productRequest)
        {
            // Arrange
            categoryRequest.OutOfStockCount = 0;
            categoryRequest.LowStockCount = 5;

            await client.PostAsJsonAsync("api/category/", categoryRequest);

            var category = client.GetFromJsonAsync<IEnumerable<Category>>("api/category/all").Result
                .FirstOrDefault(x => x.OutOfStockCount == categoryRequest.OutOfStockCount 
                    && x.LowStockCount == categoryRequest.LowStockCount);

            productRequest.CategoryId = category.Id;

            productRequest.Stock = 6;

            await client.PostAsJsonAsync("api/product", productRequest);

            var product = client.GetFromJsonAsync<IEnumerable<Product>>("api/product/all").Result
                .FirstOrDefault(x => x.CategoryId == productRequest.CategoryId && x.Stock == productRequest.Stock);

            orderRequest.Count = 8;
            orderRequest.OutOfStockMode = OutOfStockMode.ReserveWhenAvailable;
            orderRequest.ProductId = product.Id;
            orderRequest.UserId = 4;

            // Act
            await client.PostAsJsonAsync("api/order", orderRequest);

            var createdOrder = client.GetFromJsonAsync<IEnumerable<Order>>("api/order/all").Result
                .FirstOrDefault(x => x.UserId == orderRequest.UserId && x.Count == orderRequest.Count && x.ProductId == product.Id);

            await client.PutAsJsonAsync($"api/product/change-stock/{product.Id}", 15);

            Thread.Sleep(1_000);

            var updatedOrder = await client.GetFromJsonAsync<Order>($"api/order/{createdOrder.Id}");

            // Assert
            Assert.NotNull(updatedOrder);
            Assert.Equal(OrderStatus.Approved, updatedOrder.OrderStatus);
            Assert.Equal(orderRequest.Count, updatedOrder.Count);
            Assert.Equal(orderRequest.UserId, updatedOrder.UserId);
        }

        [Theory, AutoMoqData]
        public async void CreateOrder_OutOfStockModeNone_OrderNotCreated(
            CreateOrderRequest orderRequest,
            CreateCategoryRequest categoryRequest,
            CreateProductRequest productRequest)
        {
            // Arrange
            categoryRequest.OutOfStockCount = 1;
            categoryRequest.LowStockCount = 7;

            await client.PostAsJsonAsync("api/category/", categoryRequest);

            var category = client.GetFromJsonAsync<IEnumerable<Category>>("api/category/all").Result
                .FirstOrDefault(x => x.OutOfStockCount == categoryRequest.OutOfStockCount
                    && x.LowStockCount == categoryRequest.LowStockCount);

            productRequest.CategoryId = category.Id;

            productRequest.Stock = 10;

            await client.PostAsJsonAsync("api/product", productRequest);

            var product = client.GetFromJsonAsync<IEnumerable<Product>>("api/product/all").Result
                .FirstOrDefault(x => x.CategoryId == productRequest.CategoryId && x.Stock == productRequest.Stock);

            orderRequest.Count = 15;
            orderRequest.OutOfStockMode = OutOfStockMode.None;
            orderRequest.ProductId = product.Id;
            orderRequest.UserId = 5;

            // Act
            await client.PostAsJsonAsync("api/order", orderRequest);

            var createdOrder = client.GetFromJsonAsync<IEnumerable<Order>>("api/order/all").Result
                .FirstOrDefault(x => x.UserId == orderRequest.UserId && x.Count == orderRequest.Count && x.ProductId == product.Id);

            // Assert
            Assert.Null(createdOrder);
        }
    }
}