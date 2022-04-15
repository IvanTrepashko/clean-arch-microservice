using System;
using WarehouseService.Domain.Entities;
using WarehouseService.Domain.Enums;
using Xunit;

namespace WarehouseService.UnitTests.Domain.Entities
{
    public class OrderTests
    {
        [Fact]
        public void CreateOrder_InvalidCount_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Order(1, 1, 1, -5, OrderStatus.Approved));
        }
    }
}
