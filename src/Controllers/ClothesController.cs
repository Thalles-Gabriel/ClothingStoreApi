using ClothingStore.API.Data.Repositories;
using ClothingStore.API.Models;
using Microsoft.AspNetCore.Mvc;
namespace ClothingStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClothesController : ControllerBase
{
    private readonly IClothesRepository _repository;

    public ClothesController(IClothesRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<Clothes>> GetClothingList()
    {
        return Ok(await _repository.GetClothes());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Clothes>> GetClothingById([FromRoute] Guid Id)
    {
        var result = await _repository.GetClothesById(Id);
        if(result is null) return NotFound("Não existe roupa com o Id informado.");
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Clothes>> InsertClothes([FromBody] Clothes clothing)
    {
        await _repository.AddClothing(clothing);
        return CreatedAtAction(nameof(GetClothingById),
                                new { id = clothing.Id },
                                clothing);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateClothing([FromBody] Clothes clothing)
    {
        await _repository.UpdateClothing(clothing);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleleClothing([FromRoute] Guid Id)
    {
        await _repository.DeleteClothing(Id);
        return NoContent();
    }
}
