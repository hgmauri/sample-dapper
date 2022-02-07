using Sample.Dapper.Persistence.Entities;
using Sample.Dapper.Persistence.Interfaces;

namespace Sample.Dapper.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(string connectionString) 
        : base(connectionString)
    {

    }
}