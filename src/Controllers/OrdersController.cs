using ClothingStore.API.Data.Repositories;
using ClothingStore.API.Models;
using Microsoft.AspNetCore.Mvc;
namespace ClothingStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersRepository _orderRepository;
    private readonly IClothesRepository _clothesRepository;

    public OrdersController(IOrdersRepository orderRepository, IClothesRepository clothesRepository)
    {
        _orderRepository = orderRepository;
        _clothesRepository = clothesRepository;
    }

    [HttpGet]
    public async Task<ActionResult<Orders>> GetOrders()
    {
        return Ok(await _orderRepository.GetOrders());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Orders>> GetOrderById([FromRoute] Guid Id)
    {
        var result = await _orderRepository.GetOrderById(Id);
        if (result is null) return NotFound("Não existe venda com o Id informado.");
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> InsertOrder([FromBody] InsertUpdateOrderView orderView)
    {
        var order = new Orders()
        {
            Id = orderView.Id,
            User_Id = orderView.User_Id,
            PayMethod = orderView.PayMethod,
            OrderAddress = orderView.OrderAddress,
            LastChanged = DateTime.Now
        };

        await _orderRepository.AddOrder(order);
        return CreatedAtAction(nameof(GetOrderById),
                new { id = order.Id },
                order);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrder([FromBody] InsertUpdateOrderView orderView)
    {
        var order = new Orders()
        {
            Id = orderView.Id,
            User_Id = orderView.User_Id,
            PayMethod = orderView.PayMethod,
            OrderAddress = orderView.OrderAddress,
            LastChanged = DateTime.Now,
        };

        await _orderRepository.UpdateOrder(order);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder([FromRoute] Guid Id)
    {
        await _orderRepository.DeleteOrder(Id);
        return NoContent();
    }

}
