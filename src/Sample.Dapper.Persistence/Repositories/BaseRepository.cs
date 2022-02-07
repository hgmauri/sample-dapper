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

    public long Add(TEntity entity)
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.Insert(entity);
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(ConnectionString);
        var entity = connection.Get<TEntity>(id);
        connection.Delete(entity);
    }

    public bool Update(TEntity entity)
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.Update(entity);
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

    public TEntity Get(int id)
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.Get<TEntity>(id);
    }

    public IEnumerable<TEntity> Get(IEnumerable<int> entityIds)
    {
        var predicateGroup = new PredicateGroup
        {
            Operator = GroupOperator.Or,
            Predicates = entityIds.Select(e => { return Predicates.Field<TEntity>(dp => dp.EntityId, Operator.Eq, e); }).ToList<IPredicate>()
        };
        return GetByPredicate(predicateGroup);
    }

    public TEntity Get(long id)
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.Get<TEntity>(id);
    }

    public IEnumerable<TEntity> GetByPredicate(IFieldPredicate predicate)
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.GetPage<TEntity>(predicate, PageSort, PageOffset, PageLimit).ToList();
    }

    public IEnumerable<TEntity> GetByPredicate(IPredicateGroup predicateGroup)
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.GetPage<TEntity>(predicateGroup, PageSort, PageOffset, PageLimit).ToList();
    }

    public IEnumerable<TEntity> GetAll()
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.GetPage<TEntity>(null, PageSort, PageOffset, PageLimit).ToList();
    }

    public long Count(IFieldPredicate predicate)
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.Count<TEntity>(predicate);
    }

    public long CountByPredicateGroup(IPredicateGroup predicateGroup)
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.Count<TEntity>(predicateGroup);
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
        return await Task.Run(GetAll, cancellationToken).ConfigureAwait(false);
    }

    public TEntity ExecuteSingle(string query, object? parameters = null)
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.QuerySingle<TEntity>(query, parameters, commandType: CommandType.Text);
    }

    public TEntity ExecuteSingleOrDefault(string query, object? parameters = null)
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.QuerySingleOrDefault<TEntity>(query, parameters, commandType: CommandType.Text);
    }

    public IEnumerable<TEntity> Execute(string query, object? parameters = null, CommandType commandType = CommandType.Text, int commandTimeOut = 60)
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.Query<TEntity>(query, parameters, commandType: commandType, commandTimeout: commandTimeOut);
    }

    public T ExecuteSingle<T>(string query, object? parameters = null, CommandType commandType = CommandType.Text, int commandTimeOut = 60)
    {
        using var connection = new SqlConnection(ConnectionString);
        return connection.QuerySingle<T>(query, parameters, commandType: commandType, commandTimeout: commandTimeOut);
    }
}