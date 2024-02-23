using ClothingStore.API.Data.Repositories;
using ClothingStore.API.Models;
using Microsoft.AspNetCore.Mvc;
namespace ClothingStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
     private readonly IUsersRepository _userRepository;

    public UsersController(IUsersRepository repository)
    {
        _userRepository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<Users>> GetUsers()
    {
        return Ok(await _userRepository.GetUsers());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Users>> GetUserById([FromRoute] Guid Id)
    {
        var result = await _userRepository.GetUserById(Id);
        if(result is null) return NotFound("Não existe usuário com o Id informado.");
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> InsertUser([FromBody] Users user)
    {
        await _userRepository.AddUser(user);
        return CreatedAtAction(nameof(GetUserById),
                                new { id = user.Id },
                                user);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] Users user)
    {
        await _userRepository.UpdateUser(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleleUser([FromRoute] Guid Id)
    {
        await _userRepository.DeleteUser(Id);
        return NoContent();
    }


}
