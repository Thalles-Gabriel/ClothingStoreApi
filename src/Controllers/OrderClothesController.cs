using ClothingStore.API.Data.Repositories;
using ClothingStore.API.Filters.Actions;
using ClothingStore.API.Models;
using Microsoft.AspNetCore.Mvc;
namespace ClothingStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderClothesController : ControllerBase
{
    private readonly IOrderClothesRepository _repository;

    public OrderClothesController(IOrderClothesRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<OrderClothes>> GetOrderClothes()
    {
        return Ok(await _repository.GetOrderClothes());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderClothes>> GetOrderClothesById([FromRoute] Guid Id)
    {
        var result = await _repository.GetOrderClothesById(Id);
        if (result is null) return NotFound("Não existe esse item na venda com o Id informado.");
        return Ok(result);
    }

    [ServiceFilter(typeof(CheckDataDependenciesFilter))]
    [CheckClothesQuantityFilter]
    [HttpPost]
    public async Task<ActionResult<OrderClothes>> InsertOrderClothes([FromBody] OrderClothes orderClothes)
    {
        await _repository.AddOrderClothes(orderClothes);
        return CreatedAtAction(nameof(GetOrderClothesById),
                                new { id = orderClothes.Id },
                                orderClothes);
    }

    [ServiceFilter(typeof(CheckDataDependenciesFilter))]
    [HttpPut]
    public async Task<IActionResult> UpdateOrderClothes([FromBody] OrderClothes orderClothes)
    {
        await _repository.UpdateOrderClothes(orderClothes);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderClothes([FromRoute] Guid Id)
    {
        await _repository.DeleteOrderClothes(Id);
        return NoContent();
    }
}
