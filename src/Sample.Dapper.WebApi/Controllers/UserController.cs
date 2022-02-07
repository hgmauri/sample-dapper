using DapperExtensions;
using DapperExtensions.Predicate;
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

    [HttpGet("GetByPredicate")]
    public Task<IActionResult> GetByPredicate([FromQuery] int id)
    {
        var predicate = Predicates.Field<User>(f => f.EntityId, Operator.Eq, id);
        var list = _repository.GetByPredicate(predicate);
        return Task.FromResult<IActionResult>(Ok(list));
    }
}