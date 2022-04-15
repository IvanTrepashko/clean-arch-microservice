using AutoFixture.Xunit2;
using Moq;
using WarehouseService.Domain.Abstractions;
using WarehouseService.Domain.Entities;
using WarehouseService.Domain.Enums;
using WarehouseService.Domain.Services;
using WarehouseService.UnitTests.Setup;
using Xunit;

namespace WarehouseService.UnitTests.Domain.Services
{
    public class ProductServiceTests
    {
        [Theory, AutoMoqData]
        public void GetStockThresholdAsync_ReturnsValidThreshold(
            Product product,
            [Frozen] Mock<ICategoryRepository> categoryRepository,
            ProductService sut
            )
        {
            // Arrange
            product.Stock = 5;
            var category = new Category(1, "test", 0, 10);
            categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<uint>())).ReturnsAsync(category);

            // Act
            var actual = sut.GetStockThresholdAsync(product).Result;

            // Assert
            Assert.Equal(StockThreshold.LowStock, actual);
        }
    }
}
