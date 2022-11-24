﻿using BlazorAppWeb.Server.Services.CartService;
using BlazorAppWeb.Shared;
using BlazorAppWeb.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = await cartService.GetCartProducts(cartItems);
            return Ok(result);
        }
    }
}
