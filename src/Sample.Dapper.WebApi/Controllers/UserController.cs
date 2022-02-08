using Sample.Dapper.Persistence.Entities;
using Sample.Dapper.Persistence.Interfaces;

namespace Sample.Dapper.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Post([FromBody] User user)
    {
        var result = await _repository.AddAsync(user);
        return Ok(result);
    }

    [HttpGet("Get")]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        var result = await _repository.GetAsync(id);
        return Ok(result);
    }

    [HttpGet("GetListPaged")]
    public async Task<IActionResult> GetListPaged(int pageNumber, int rowPerPages, string conditions, string orderby)
    {
        var list = await _repository.GetListPaged(pageNumber, rowPerPages, conditions, orderby);
        return Ok(list);
    }
}