using GoodHamburger.Application.Commands.Orders;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Queries.Orders;
using GoodHamburger.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all orders with pagination
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<OrderResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            var query = new GetAllOrdersQuery { Page = page, PageSize = pageSize };
            var orders = await _mediator.Send(query, cancellationToken);
            return Ok(orders);
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var query = new GetOrderByIdQuery { Id = id };
            var order = await _mediator.Send(query, cancellationToken);
            if (order == null)
                return NotFound(new { Message = $"Order with id {id} not found" });

            return Ok(order);
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateOrderCommand 
            { 
                CustomerName = request.CustomerName,
                Items = request.Items
            };
            var order = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        /// <summary>
        /// Update an existing order
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateOrderCommand 
            { 
                Id = id,
                CustomerName = request.CustomerName,
                Items = request.Items
            };
            var order = await _mediator.Send(command, cancellationToken);
            return Ok(order);
        }

        /// <summary>
        /// Delete an order (soft delete)
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteOrderCommand { Id = id };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Update order status
        /// </summary>
        [HttpPut("{id:int}/status")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateOrderStatusCommand 
            { 
                Id = id,
                Status = (OrderStatus)request.Status
            };
            var order = await _mediator.Send(command, cancellationToken);
            return Ok(order);
        }
    }
}
