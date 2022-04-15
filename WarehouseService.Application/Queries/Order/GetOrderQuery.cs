using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Application.Queries
{
    public class GetOrderQuery : IRequest<Order>
    {
        public uint OrderId { get; init; }

        public GetOrderQuery(uint orderId)
        {
            OrderId = orderId;
        }
    }
}
