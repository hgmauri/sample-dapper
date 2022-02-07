using System.Data;
using DapperExtensions.Predicate;
using Sample.Dapper.Persistence.Entities;

namespace Sample.Dapper.Persistence.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    long Add(TEntity entity);
    Task<long> AddAsync(TEntity entity);
    Task<long> AddAsync(TEntity entity, CancellationToken cancellationToken);
    long Count(IFieldPredicate predicate);
    long CountByPredicateGroup(IPredicateGroup predicateGroup);
    void Delete(int id);
    Task DeleteAsync(int id);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    IEnumerable<TEntity> Execute(string query, object? parameters = null, CommandType commandType = CommandType.Text, int commandTimeOut = 60);
    TEntity ExecuteSingle(string query, object? parameters = null);
    T ExecuteSingle<T>(string query, object? parameters = null, CommandType commandType = CommandType.Text, int commandTimeOut = 60);
    TEntity ExecuteSingleOrDefault(string query, object? parameters = null);
    IEnumerable<TEntity> Get(IEnumerable<int> entityIds);
    TEntity Get(int id);
    TEntity Get(long id);
    IEnumerable<TEntity> GetAll();
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity> GetAsync(int id);
    Task<TEntity> GetAsync(int id, CancellationToken cancellationToken);
    IEnumerable<TEntity> GetByPredicate(IFieldPredicate predicate);
    IEnumerable<TEntity> GetByPredicate(IPredicateGroup predicateGroup);
    void SetPageLimit(int pageLimit);
    void SetPageOffset(int pageOffset);
    void SetPageSort(IList<ISort> sortData);
    void SetPagingParameters(int pageOffset, int pageLimit);
    void SetPagingParameters(int pageOffset, int pageLimit, IList<ISort> sortData);
    bool Update(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}