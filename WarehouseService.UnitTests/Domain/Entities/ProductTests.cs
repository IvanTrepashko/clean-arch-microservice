using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseService.Domain.Entities;
using WarehouseService.UnitTests.Setup;
using Xunit;

namespace WarehouseService.UnitTests.Domain.Entities
{
    public class ProductTests
    {
        [Theory, AutoMoqData]
        public void SetStock_ValidValue([NoAutoProperties]Product product)
        {
            // Act 
            product.Stock = 50;

            // Assert
            Assert.Equal(50, product.Stock);
        }

        [Theory, AutoMoqData]
        public void SetStock_InvalidValue_ThrowsArgumentOutOfRangeException([NoAutoProperties] Product product)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => product.Stock = -50);
        }

        [Fact]
        public void CreateProduct_InvalidStock_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Product(1, "test", -5, 1));
        }
    }
}
