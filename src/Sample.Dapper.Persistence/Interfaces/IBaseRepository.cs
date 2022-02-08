using System.Data;
using Sample.Dapper.Persistence.Entities;

namespace Sample.Dapper.Persistence.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<int?> Add(TEntity entity);
    Task Delete(int id);
    Task<int> Update(TEntity entity);
    Task<TEntity> Get(int id);
    Task<TEntity> Get(long id);
    Task<IEnumerable<TEntity>> GetAll();
    Task<IEnumerable<TEntity>> GetListPaged(int pageNumber, int rowPerPages, string conditions, string orderby);
    Task<long> Count(string conditions);
    Task<int?> AddAsync(TEntity entity);
    Task<int?> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(int id);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<int> UpdateAsync(TEntity entity);
    Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity> GetAsync(int id);
    Task<TEntity> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity> ExecuteSingle(string query, object? parameters = null);
    Task<T> ExecuteSingle<T>(string query, object? parameters = null, CommandType commandType = CommandType.Text, int commandTimeOut = 60);
    Task<TEntity> ExecuteSingleOrDefault(string query, object? parameters = null);
    Task<IEnumerable<TEntity>> Execute(string query, object? parameters = null, CommandType commandType = CommandType.Text, int commandTimeOut = 60);
}