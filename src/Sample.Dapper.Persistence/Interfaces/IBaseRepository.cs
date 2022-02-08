using System.Data;
using DapperExtensions.Predicate;
using Sample.Dapper.Persistence.Entities;

namespace Sample.Dapper.Persistence.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<long> Add(TEntity entity);
    Task<long> AddAsync(TEntity entity);
    Task<long> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<long> Count(IFieldPredicate predicate);
    Task<long> CountByPredicateGroup(IPredicateGroup predicateGroup);
    Task Delete(int id);
    Task DeleteAsync(int id);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> Execute(string query, object? parameters = null, CommandType commandType = CommandType.Text, int commandTimeOut = 60);
    Task<TEntity> ExecuteSingle(string query, object? parameters = null);
    Task<T> ExecuteSingle<T>(string query, object? parameters = null, CommandType commandType = CommandType.Text, int commandTimeOut = 60);
    Task<TEntity> ExecuteSingleOrDefault(string query, object? parameters = null);
    Task<IEnumerable<TEntity>> Get(IEnumerable<int> entityIds);
    Task<TEntity> Get(int id);
    Task<TEntity> Get(long id);
    Task<IEnumerable<TEntity>> GetAll();
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity> GetAsync(int id);
    Task<TEntity> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetByPredicate(IFieldPredicate predicate);
    Task<IEnumerable<TEntity>> GetByPredicate(IPredicateGroup predicateGroup);
    void SetPageLimit(int pageLimit);
    void SetPageOffset(int pageOffset);
    void SetPageSort(IList<ISort> sortData);
    void SetPagingParameters(int pageOffset, int pageLimit);
    void SetPagingParameters(int pageOffset, int pageLimit, IList<ISort> sortData);
    Task<bool> Update(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}