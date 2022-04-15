using AutoMapper;
using WarehouseService.API.ApiModels;
using WarehouseService.Domain.Entities;

namespace WarehouseService.API.Mappings
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<CategoryModel, Category>();
            CreateMap<ProductModel, Product>();
        }
    }
}
