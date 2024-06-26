﻿using BlazorAppWeb.Server.Services.OrderService;
using BlazorAppWeb.Shared;
using BlazorAppWeb.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder()
        {
            var result = await orderService.PlaceOrder();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewResponse>>>> GetOrders()
        {
            var result = await orderService.GetOrders();
            return Ok(result);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<OrderDetailsResponse>>> GetOrderDetails(int orderId)
        {
            var result = await orderService.GetOrderDetails(orderId);
            return Ok(result);
        }
    }
}
