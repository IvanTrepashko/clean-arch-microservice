using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseService.Application.Queries
{
    public class GetOrdersByUserIdQuery : IRequest<GetOrdersResponse>
    {
        public uint UserId { get; init; }

        public GetOrdersByUserIdQuery(uint userId)
        {
            UserId = userId;
        }
    }
}
