using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Filters;
using SampleAPI.Models;
using SampleAPI.Repositories;
using SampleAPI.Requests;

namespace SampleAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository ordersRepository)
        {
            _orderRepository = ordersRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Map(_orderRepository.Get()));
        }

        [HttpGet("{id:guid}")]
        [OrderExists]
        public IActionResult GetById(Guid id)
        {
            return Ok(Map(_orderRepository.Get(id)));
        }

        [HttpPost]
        public IActionResult Post(OrderRequest request)
        {
            var order = Map(request);
            _orderRepository.Add(order);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, null);
        }

        [HttpPut("{id:guid}")]
        [OrderExists]
        public IActionResult Put(Guid id, OrderRequest request)
        {
            if (request.ItemsIds == null)        
            {        
                return BadRequest();
            }
            
            var order = _orderRepository.Get(id);
            
            if (order == null)        
            {        
                return NotFound(new { Message = $"Item with id {id} not exist." });
            }
            
            order = Map(request, order);

            _orderRepository.Update(id, order);      
            return Ok();  
        }
        
        [HttpPatch("{id:guid}")] 
        [OrderExists]
        public IActionResult Patch(Guid id, JsonPatchDocument<Order> requestOp)
        {
            var order = _orderRepository.Get(id);
            if (order == null)
            {
                return NotFound(new { Message = $"Item with id {id} not exist." });
            }

            requestOp.ApplyTo(order);
            _orderRepository.Update(id, order);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [OrderExists]
        public IActionResult Delete(Guid id)
        {
            var order = _orderRepository.Get(id);

            if (order == null)
            {
                return NotFound(new { Message = $"Item with id {id} not exist." });
            }

            _orderRepository.Delete(id);
            return NoContent();
        }
        
        private Order Map(OrderRequest request)
        {

            return new Order
            {
                Id = Guid.NewGuid(),
                ItemsIds = request.ItemsIds,
                Currency = request.Currency
            };
        }
        
        private Order Map(OrderRequest request, Order order)
        {
            order.ItemsIds = request.ItemsIds;
            order.Currency = request.Currency;
 
            return order;
        } 
        
        private IList<OrderResponse> Map(IList<Order> orders)
        {
            return orders.Select(Map).ToList();
        }
        
        private OrderResponse Map(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                ItemsIds = order.ItemsIds,
                Currency = order.Currency
            };
        }
    }
}