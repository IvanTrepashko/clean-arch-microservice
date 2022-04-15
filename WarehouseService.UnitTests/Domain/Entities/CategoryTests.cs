using System;
using WarehouseService.Domain.Entities;
using Xunit;

namespace WarehouseService.UnitTests.Domain.Entities
{
    public class CategoryTests
    {
        [Fact]
        public void CreateCategory_OutOfStockCountIsNegative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var category = new Category(1, "test", -4, 0);
            });
        }

        [Fact]
        public void CreateCategory_LowStockCountIsNegative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var category = new Category(1, "test", 4, -4);
            });
        }

        [Fact]
        public void CreateCategory_OutOfStockCountMoreThanLowStockCount_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var category = new Category(1, "test", 10, 0);
            });
        }
    }
}
