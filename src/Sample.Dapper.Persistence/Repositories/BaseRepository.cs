using System.Data;
using System.Data.SqlClient;
using Dapper;
using DapperExtensions;
using DapperExtensions.Predicate;
using Sample.Dapper.Persistence.Entities;
using Sample.Dapper.Persistence.Interfaces;

namespace Sample.Dapper.Persistence.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected string ConnectionString { get; set; }
    private IList<ISort> PageSort { get; set; }
    private int PageOffset { get; set; }
    private int PageLimit { get; set; }

    protected BaseRepository(string connectionString)
    {
        ConnectionString = connectionString;
        PageSort = new List<ISort> {Predicates.Sort<TEntity>(entity => entity.EntityId, false)};
        PageOffset = 0;
        PageLimit = 1000;
    }

    public async Task<long> Add(TEntity entity)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = connection.Insert(entity);
        await connection.CloseAsync();
        return result;
    }

    public async Task Delete(int id)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var entity = connection.Get<TEntity>(id);
        connection.Delete(entity);
        await connection.CloseAsync();
    }

    public async Task<bool> Update(TEntity entity)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = connection.Update(entity);
        await connection.CloseAsync();
        return result;
    }

    public void SetPageOffset(int pageOffset)
    {
        PageOffset = pageOffset;
    }

    public void SetPageLimit(int pageLimit)
    {
        PageLimit = pageLimit;
    }

    public void SetPageSort(IList<ISort> sortData)
    {
        PageSort = sortData;
    }

    public void SetPagingParameters(int pageOffset, int pageLimit, IList<ISort> sortData)
    {
        PageOffset = pageOffset;
        PageLimit = pageLimit;
        PageSort = sortData;
    }

    public void SetPagingParameters(int pageOffset, int pageLimit)
    {
        PageOffset = pageOffset;
        PageLimit = pageLimit;
    }

    public async Task<TEntity> Get(int id)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = await connection.GetAsync<TEntity>(id);
        await connection.CloseAsync();
        return result;
    }

    public async Task<IEnumerable<TEntity>> Get(IEnumerable<int> entityIds)
    {
        var predicateGroup = new PredicateGroup
        {
            Operator = GroupOperator.Or,
            Predicates = entityIds.Select(e => { return Predicates.Field<TEntity>(dp => dp.EntityId, Operator.Eq, e); }).ToList<IPredicate>()
        };
        return await GetByPredicate(predicateGroup);
    }

    public async Task<TEntity> Get(long id)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = connection.Get<TEntity>(id);
        await connection.CloseAsync();
        return result;
    }

    public async Task<IEnumerable<TEntity>> GetByPredicate(IFieldPredicate predicate)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = connection.GetPage<TEntity>(predicate, PageSort, PageOffset, PageLimit).ToList();
        await connection.CloseAsync();
        return result;
    }

    public async Task<IEnumerable<TEntity>> GetByPredicate(IPredicateGroup predicateGroup)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = connection.GetPage<TEntity>(predicateGroup, PageSort, PageOffset, PageLimit).ToList();
        await connection.CloseAsync();
        return result;
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = connection.GetPage<TEntity>(null, PageSort, PageOffset, PageLimit).ToList();
        await connection.CloseAsync();
        return result;
    }

    public async Task<long> Count(IFieldPredicate predicate)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = connection.Count<TEntity>(predicate);
        await connection.CloseAsync();
        return result;
    }

    public async Task<long> CountByPredicateGroup(IPredicateGroup predicateGroup)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = connection.Count<TEntity>(predicateGroup);
        await connection.CloseAsync();
        return result;
    }

    public virtual async Task<long> AddAsync(TEntity entity)
    {
        return await AddAsync(entity, CancellationToken.None).ConfigureAwait(false);
    }

    public virtual async Task<long> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return await Task.Run(() => Add(entity), cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task DeleteAsync(int id)
    {
        await DeleteAsync(id, CancellationToken.None).ConfigureAwait(false);
    }

    public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await Task.Run(() => Delete(id), cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        return await Task.Run(() => Update(entity), CancellationToken.None).ConfigureAwait(false);
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return await Task.Run(() => Update(entity), cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<TEntity> GetAsync(int id)
    {
        return await GetAsync(id, CancellationToken.None).ConfigureAwait(false);
    }

    public virtual async Task<TEntity> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await Task.Run(() => Get(id), cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await GetAllAsync(CancellationToken.None).ConfigureAwait(false);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Task.Run(() => GetAll(), cancellationToken).ConfigureAwait(false);
    }

    public async Task<TEntity> ExecuteSingle(string query, object? parameters = null)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = connection.QuerySingle<TEntity>(query, parameters, commandType: CommandType.Text);
        await connection.CloseAsync();
        return result;
    }

    public async Task<TEntity> ExecuteSingleOrDefault(string query, object? parameters = null)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = connection.QuerySingleOrDefault<TEntity>(query, parameters, commandType: CommandType.Text);
        await connection.CloseAsync();
        return result;
    }

    public async Task<IEnumerable<TEntity>> Execute(string query, object? parameters = null, CommandType commandType = CommandType.Text, int commandTimeOut = 60)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = connection.Query<TEntity>(query, parameters, commandType: commandType, commandTimeout: commandTimeOut);
        await connection.CloseAsync();
        return result;
    }

    public async Task<T> ExecuteSingle<T>(string query, object? parameters = null, CommandType commandType = CommandType.Text, int commandTimeOut = 60)
    {
        await using var connection = new SqlConnection(ConnectionString);
        var result = connection.QuerySingle<T>(query, parameters, commandType: commandType, commandTimeout: commandTimeOut);
        await connection.CloseAsync();
        return result;
    }
}