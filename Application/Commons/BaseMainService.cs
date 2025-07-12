using Application.Commons.Contracts;
using AutoDependencyRegistration.Attributes;
using Domain.Commons;
using Domain.ViewModels;
using Infrastructure.Extensions;
using Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Serilog;
using System.Linq.Expressions;
using System.Reflection;

namespace Application.Commons;

[RegisterClassAsScoped]
public class BaseMainService<TDto, TEntity> : IBaseMainService<TDto, TEntity> where TDto : BaseDto where TEntity : BaseEntity
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly IDynamicMapper _dynamicMapper;
    protected readonly ILogger _logger;
    protected readonly ISessionProvider _sessionProvider;
    protected readonly DataContext _context;
    protected string CacheKey = typeof(TEntity).Name;
    protected Guid CurrentUserGuid = Guid.Empty;
    public BaseMainService(DataContext context, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _context = context;
        _dynamicMapper = _serviceProvider.GetRequiredService<IDynamicMapper>();
        _logger = _serviceProvider.GetRequiredService<ILogger>();
        _sessionProvider = _serviceProvider.GetRequiredService<ISessionProvider>();
        CurrentUserGuid = _sessionProvider.GetCurrentUserGuid();
    }
    #region Select Data
    public IEnumerable<TDto> GetAllPublished() => GetAllPublishedAsync().Result;
    public async Task<IEnumerable<TDto>> GetAllPublishedAsync() => _dynamicMapper.MapList<TEntity, TDto>(await _context.Set<TEntity>().AsNoTracking().Where(w => w.IsDeleted == false && w.IsPublished && w.IsActive).OrderBy(o => o.CreatedAt).ToListAsync());
    public PaginatedResult<TDto> GetAllPublished(int pageNumber = 1, int pageSize = 10) => GetAllPublishedAsync(pageNumber, pageSize).Result;
    public async Task<PaginatedResult<TDto>> GetAllPublishedAsync(int pageNumber = 1, int pageSize = 10)
    {
        var (Query, Total) = await GetQueryableAndTotalAsync(null);
        var items = _dynamicMapper.MapList<TEntity, TDto>(await Query.Where(w => w.IsDeleted == false && w.IsPublished && w.IsActive).OrderBy(o => o.CreatedAt).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync());
        return new PaginatedResult<TDto>(items, Total, pageNumber, pageSize);
    }
    public IEnumerable<TDto> GetAll() => GetAllAsync().Result;
    public async Task<IEnumerable<TDto>> GetAllAsync() => _dynamicMapper.MapList<TEntity, TDto>(EntitiesWithLatestVersions(await _context.Set<TEntity>().AsNoTracking().Where(w => w.IsDeleted == false).ToListAsync()));
    public PaginatedResult<TDto> GetAll(int pageNumber = 1, int pageSize = 10) => GetAllAsync(pageNumber, pageSize).Result;
    public async Task<PaginatedResult<TDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var (Query, Total) = await GetQueryableAndTotalAsync(null);
        var items = _dynamicMapper.MapList<TEntity, TDto>(EntitiesWithLatestVersions(await Query.Where(w => w.IsDeleted == false).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()));
        return new PaginatedResult<TDto>(items, Total, pageNumber, pageSize);
    }
    public IEnumerable<TDto> Search(Expression<Func<TEntity, bool>>? predicate) => SearchAsync(predicate).Result;
    public async Task<IEnumerable<TDto>> SearchAsync(Expression<Func<TEntity, bool>>? predicate) => _dynamicMapper.MapList<TEntity, TDto>(await _context.Set<TEntity>().AsNoTracking().Where(w => w.IsDeleted == false && w.IsPublished && w.IsActive).NullSafeWhere(predicate).OrderBy(o => o.CreatedAt).ToListAsync());
    public PaginatedResult<TDto> Search(Expression<Func<TEntity, bool>>? predicate, int pageNumber = 1, int pageSize = 10) => SearchAsync(predicate, pageNumber, pageSize).Result;
    public async Task<PaginatedResult<TDto>> SearchAsync(Expression<Func<TEntity, bool>>? predicate, int pageNumber = 1, int pageSize = 10)
    {
        var (Query, Total) = await GetQueryableAndTotalAsync(predicate);
        var items = _dynamicMapper.MapList<TEntity, TDto>(await Query.NullSafeWhere(predicate).Where(w => w.IsDeleted == false && w.IsPublished && w.IsActive).OrderBy(o => o.CreatedAt).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync());
        return new PaginatedResult<TDto>(items, Total, pageNumber, pageSize);
    }
    public IEnumerable<TDto> SearchLatest(Expression<Func<TEntity, bool>>? predicate) => SearchLatestAsync(predicate).Result;
    public async Task<IEnumerable<TDto>> SearchLatestAsync(Expression<Func<TEntity, bool>>? predicate) => _dynamicMapper.MapList<TEntity, TDto>(EntitiesWithLatestVersions(await _context.Set<TEntity>().AsNoTracking().NullSafeWhere(predicate).Where(w => w.IsDeleted == false).ToListAsync()));
    public PaginatedResult<TDto> SearchLatest(Expression<Func<TEntity, bool>>? predicate, int pageNumber = 1, int pageSize = 10) => SearchLatestAsync(predicate, pageNumber, pageSize).Result;
    public async Task<PaginatedResult<TDto>> SearchLatestAsync(Expression<Func<TEntity, bool>>? predicate, int pageNumber = 1, int pageSize = 10)
    {
        var result = await GetQueryableAndTotalAsync(predicate);
        var items = _dynamicMapper.MapList<TEntity, TDto>(EntitiesWithLatestVersions(await result.Query.NullSafeWhere(predicate).Where(w => w.IsDeleted == false).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()));
        return new PaginatedResult<TDto>(items, result.Total, pageNumber, pageSize);
    }
    #endregion
    #region Select by Id
    public TDto GetById(Guid? id) => GetByIdAsync(id).Result;
    public async Task<TDto> GetByIdAsync(Guid? id) => _dynamicMapper.Map<TDto>(EntityWithLatestVersion(await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(f => f.Id == id)));
    public TDto GetPublishById(Guid? id) => GetPublishByIdAsync(id).Result;
    public async Task<TDto> GetPublishByIdAsync(Guid? id) => _dynamicMapper.Map<TDto>(await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(f => f.Id == id.GetValueOrDefault()));
    #endregion
    #region CRUD Operations
    public ResultResponse Save(TDto model, bool isAutoSort = false) => SaveToDatabase(new List<TDto> { model }, isAutoSort);
    public async Task<ResultResponse> SaveAsync(TDto model, bool isAutoSort = false) => await SaveToDatabaseAsync(new List<TDto> { model }, isAutoSort);
    public ResultResponse Save(TDto model, ModelStateDictionary modelState) => (!modelState.IsValid) ? ResultResponse.Set(false, Resources.PleaseCorrectTheValidationErrors, modelState) : SaveToDatabase(new List<TDto> { model });
    public async Task<ResultResponse> SaveAsync(TDto model, ModelStateDictionary modelState) => (!modelState.IsValid) ? ResultResponse.Set(false, Resources.PleaseCorrectTheValidationErrors, modelState) : await SaveToDatabaseAsync(new List<TDto> { model });
    internal ResultResponse SaveToDatabase(List<TDto> models, bool isAutoSort = true) => SaveToDatabaseAsync(models, isAutoSort).Result;
    internal async Task<ResultResponse> SaveToDatabaseAsync(List<TDto> models, bool isAutoSort = true)
    {
        try
        {
            TEntity? result = null;
            foreach (var model in models)
            {

                if (model.Id == Guid.Empty)
                {
                    if (isAutoSort)
                        model.Sort = GetSort(model.Sort);

                    TEntity entity = _dynamicMapper.Map<TEntity>(model);
                    entity.CreatedBy = CurrentUserGuid;
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.IsPublished = _sessionProvider.HasPublishPermission;
                    if (_sessionProvider.HasPublishPermission)
                    {
                        entity.PublishedBy = CurrentUserGuid;
                        entity.PublishedAt = DateTime.UtcNow;
                    }
                    _context.Set<TEntity>().Add(entity);
                    _context.SaveChanges();
                    result = entity;
                }
                else
                {
                    if (_sessionProvider.HasPublishPermission)
                    {
                        var entity = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(f => f.Id == model.Id);
                        if (entity != null)
                        {
                            var originalCreatedAt = entity.CreatedAt;
                            entity = _dynamicMapper.Map<TEntity>(model);
                            entity.CreatedAt = originalCreatedAt;
                            entity.IsPublished = true;
                            entity.PublishedBy = CurrentUserGuid;
                            entity.PublishedAt = DateTime.UtcNow;
                            entity.ModifiedBy = CurrentUserGuid;
                            entity.ModifiedAt = DateTime.UtcNow;
                            _context.Set<TEntity>().Update(entity);
                            _context.SaveChanges();
                            model.Id = entity.Id;
                        }
                    }
                    result = _dynamicMapper.Map<TEntity>(model);
                    AddVersion(model);
                    RemoveOldHistoryVersions(model.Id);
                }
            }
            return ResultResponse.Set(true, Resources.DataSavedSuccessfully, model: result);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ResultResponse.Set(false, ex.Message);
        }
    }
    public async Task<ResultResponse> DeleteAsync(Guid id) => await UpdateIsDeletedColumnAsync(id, true, Resources.DataDeletedSuccessfully);
    public async Task<ResultResponse> DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            await DeleteAsync(entity.Id);

        return ResultResponse.Set(false, Resources.DataDeletedSuccessfully);
    }
    public async Task<ResultResponse> RestoreAsync(Guid id) => await UpdateIsDeletedColumnAsync(id, false, Resources.DataRestoredSuccessfully);
    public async Task<ResultResponse> ResortAsync(Guid[] ids)
    {
        int counter = 1;
        foreach (var id in ids)
        {
            var model = _context.Set<TEntity>().FirstOrDefault(f => f.Id == id);
            if (model is not null)
            {
                model.Sort = counter++;
                _context.Set<TEntity>().Update(model);
                await _context.SaveChangesAsync();
            }
        }
        return ResultResponse.Set(true, Resources.DataSavedSuccessfully);
    }
    public async Task<ResultResponse> ResortAsync(Guid[] ids, int pageNumber, int pageSize)
    {
        try
        {
            // Calculate the base sort number for the current page
            int baseSort = (pageNumber - 1) * pageSize + 1;
            int counter = 0;

            var entities = await _context.Set<TEntity>()
                .Where(e => ids.Contains(e.Id) && !e.IsDeleted)
                .ToListAsync();

            foreach (var id in ids)
            {
                var entity = entities.FirstOrDefault(e => e.Id == id);
                if (entity != null)
                {
                    entity.Sort = baseSort + counter;
                    counter++;
                }
            }

            _context.Set<TEntity>().UpdateRange(entities);
            await _context.SaveChangesAsync();

            return ResultResponse.Set(true, Resources.DataSavedSuccessfully);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ResultResponse.Set(false, ex.Message);
        }
    }



    public async Task<int> FindAndReplaceAsync(string oldKeyword, string newKeyword)
    {
        var result = _context.Set<TEntity>().SearchAllColumns(oldKeyword);
        result.ReplaceAllColumns(oldKeyword, newKeyword);
        await _context.SaveChangesAsync();
        return result.Count();
    }

    #endregion

    #region Select Publish and Delete Requests and Guids


    public List<Guid> GetNeedPublishRequestsGuids() => GetNeedPublishRequestsGuids(null);
    public List<Guid> GetNeedPublishRequestsGuids(Expression<Func<TEntity, bool>>? predicate)
    {
        var unpublishedEntities = _context.Set<TEntity>()
            .AsNoTracking()
            .NullSafeWhere(predicate)
            .Where(w => !w.IsDeleted && !w.IsPublished)
            .Select(s => s.Id)
            .ToHashSet();

        var unpublishedVersions = GetBaseVersion()?
            .AsNoTracking()
            .Where(v => !v.IsDeleted && !v.IsPublished)
            .Select(v => v.MainVersionId)
            .Distinct()
            .ToList() ?? new List<Guid>();

        unpublishedEntities.UnionWith(unpublishedVersions);

        return unpublishedEntities.Where(w => IsLatestVersion(w)).ToList() ?? new List<Guid>();
    }
    public List<Guid> GetNeedDeleteRequestsGuids() => GetNeedDeleteRequestsGuids(null);
    public List<Guid> GetNeedDeleteRequestsGuids(Expression<Func<TEntity, bool>>? predicate) => _context.Set<TEntity>()
            .AsNoTracking()
            .Where(e => !e.IsDeleted && e.IsNeedDeleteApprove)
            .NullSafeWhere(predicate)
            .Select(e => e.Id)
            .ToList() ?? new List<Guid>();

    #endregion

    #region Publish/Delete Request Models
    public IEnumerable<TDto> GetPublishAndDeleteRequests() => GetPublishAndDeleteRequests(null);
    public IEnumerable<TDto> GetPublishAndDeleteRequests(Expression<Func<TEntity, bool>>? predicate)
    {
        var result = new List<TDto>();
        var needPublishGuids = GetNeedPublishRequestsGuids(predicate);
        result.AddRange(needPublishGuids.Select(s => GetById(s)));
        var needDeleteGuids = GetNeedDeleteRequestsGuids(predicate);
        result.AddRange(needDeleteGuids.Select(s => GetById(s)));
        return result.OrderByDescending(o => o.ModifiedAt);
    }
    #endregion

    #region Content Counts
    public long GetCount() => GetCount(null);
    public long GetCount(Expression<Func<TEntity, bool>>? predicate) => _context.Set<TEntity>().AsNoTracking().NullSafeWhere(predicate).Count(w => w.IsDeleted == false);
    public int GetPublishAndDeleteRequestCount() => GetPublishRequestCount() + GetDeleteRequestCount();
    public int GetPublishRequestCount() => GetNeedPublishRequestsGuids().Count();
    public int GetDeleteRequestCount() => GetNeedDeleteRequestsGuids().Count();

    #endregion

    #region Content History
    public List<EntityHistoryViewModel> HistoryVersions(Guid id)
    {
        List<EntityHistoryViewModel> model = new();
        var entity = _context.Set<TEntity>().FirstOrDefault(f => f.Id == id);
        if (entity != null)
        {
            model.Add(new EntityHistoryViewModel { Id = id, CreatedBy = entity.CreatedBy, CreatedAt = entity.CreatedAt, IsMain = true, IsPublished = entity.IsPublished });

            if (entity.IsNeedDeleteApprove)
                model.Add(new EntityHistoryViewModel { Id = id, CreatedBy = entity.DeletedBy, CreatedAt = entity.DeletedAt, IsMain = true, IsPublished = entity.IsPublished, IsNeedDeleteApprove = entity.IsNeedDeleteApprove });
        }

        var versions = GetBaseVersion()?.AsNoTracking().Where(w => w.MainVersionId == id && w.IsDeleted == false);
        if (versions != null)
        {
            foreach (var item in versions)
                model.Add(new EntityHistoryViewModel { Id = item.Id, CreatedBy = item.CreatedBy, CreatedAt = item.CreatedAt, IsMain = false, IsPublished = item.IsPublished });
        }
        return model;
    }
    public EntityHistoryDetailsViewModel HistoryDetails(Guid id)
    {
        EntityHistoryDetailsViewModel model = new()
        {
            Current = GetPublishById(id),
            Version = GetById(id)
        };
        return model;
    }
    internal bool IsLatestVersion(Guid id)
    {
        var latestVersion = GetBaseVersion()?
            .AsNoTracking()
            .Where(w => w.MainVersionId == id && !w.IsDeleted)
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => w.IsPublished)
            .FirstOrDefault();

        // If latest version exists, return true only if it's not published
        if (latestVersion != default)
            return latestVersion == false;

        // If no version exists, check the base entity
        return _context.Set<TEntity>()
            .AsNoTracking()
            .Any(e => e.Id == id && !e.IsPublished);
    }
    internal List<TEntity> EntitiesWithLatestVersions(IEnumerable<TEntity> entities)
    {
        List<TEntity> model = new();
        foreach (var entity in entities)
        {
            var item = EntityWithLatestVersion(entity);
            if (item != null)
                model.Add(item);
        }
        return model.OrderBy(o => o.Sort).ToList() ?? new List<TEntity>();
    }
    internal TEntity? EntityWithLatestVersion(TEntity? entity)
    {
        if (entity == null) return entity;

        var latestVersion = GetBaseVersion()?.AsNoTracking().Where(w => w.MainVersionId == entity.Id && w.IsDeleted == false).OrderByDescending(o => o.CreatedAt).FirstOrDefault();
        if (latestVersion != null)
        {
            latestVersion.Id = latestVersion.MainVersionId;
            var model = _dynamicMapper.Map(latestVersion, entity);
            model.ModifiedAt = latestVersion.CreatedAt;
            model.ModifiedBy = latestVersion.CreatedBy;
            return model;
        }
        return entity;

    }
    #endregion

    #region Publish Content Records
    public ResultResponse ConfirmPublish(Guid id)
    {
        try
        {
            var versions = GetBaseVersion()?.AsNoTracking().Where(w => w.MainVersionId == id && w.IsDeleted == false).OrderByDescending(o => o.CreatedAt).ToList();
            var latest = versions?.FirstOrDefault();
            if (latest != null && !latest.IsPublished)
            {
                var entity = _context.Set<TEntity>().AsNoTracking().FirstOrDefault(f => f.Id == id);
                if (entity is null)
                    return ResultResponse.Set(false, Resources.DataNotFound);
                else
                {
                    entity = _dynamicMapper.Map(latest, entity);
                    entity.Id = latest.MainVersionId;
                    entity.IsPublished = true;
                    entity.PublishedBy = CurrentUserGuid;
                    entity.PublishedAt = DateTime.UtcNow;
                    entity.CreatedAt = latest.CreatedAt;
                    entity.CreatedBy = latest.CreatedBy;
                    entity.IsNeedDeleteApprove = false;
                    _context.Set<TEntity>().Update(entity);
                    _context.SaveChanges();

                }
            }
            else
            {
                var entity = _context.Set<TEntity>().AsNoTracking().FirstOrDefault(f => f.Id == id);
                if (entity is null)
                    return ResultResponse.Set(false, Resources.DataNotFound);
                else
                {
                    entity.IsPublished = true;
                    entity.PublishedBy = CurrentUserGuid;
                    entity.PublishedAt = DateTime.UtcNow;
                    entity.IsNeedDeleteApprove = false;
                    _context.Set<TEntity>().Update(entity);
                    _context.SaveChanges();
                }
            }

            if (versions != null)
            {
                foreach (var version in versions)
                {
                    version.IsPublished = true;
                    version.PublishedBy = CurrentUserGuid;
                    version.PublishedAt = DateTime.UtcNow;
                    UpdateVersion(version);
                }
            }
            _context.SaveChanges();
            return ResultResponse.Set(true, Resources.DataPublishedSuccessfully);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ResultResponse.Set(false, ex.Message);
        }
    }
    public ResultResponse CancelPublish(Guid id)
    {
        try
        {
            var entity = _context.Set<TEntity>().AsNoTracking().FirstOrDefault(f => f.Id == id && !f.IsPublished);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _context.Set<TEntity>().Update(entity);
                _context.SaveChanges();
            }
            else
            {
                var versions = GetBaseVersion()?.AsNoTracking().Where(w => w.MainVersionId == id && w.IsDeleted == false && w.IsPublished == false).ToList();
                if (versions != null)
                {
                    foreach (var version in versions)
                    {
                        version.IsDeleted = true;
                        UpdateVersion(version);
                        _context.SaveChanges();
                    }
                }
            }
            return ResultResponse.Set(true, Resources.DataPublishedSuccessfully);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ResultResponse.Set(false, ex.Message);
        }
    }
    public ResultResponse PublishAll()
    {
        try
        {
            var entities = GetNeedPublishRequestsGuids();
            foreach (var item in entities)
                ConfirmPublish(item);

            //_cacheProvider.Reset();
            return ResultResponse.Set(true, Resources.DataPublishedSuccessfully);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ResultResponse.Set(false, ex.Message);
        }
    }
    public ResultResponse CancelPublishAll()
    {
        try
        {
            var guids = GetNeedPublishRequestsGuids();
            foreach (var item in guids)
                CancelPublish(item);

            //_cacheProvider.Reset();
            return ResultResponse.Set(true, Resources.DataPublishedSuccessfully);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ResultResponse.Set(false, ex.Message);
        }
    }
    #endregion

    #region Delete Content Records
    public ResultResponse ConfirmDelete(Guid id)
    {
        try
        {
            var entity = _context.Set<TEntity>().AsNoTracking().FirstOrDefault(f => f.Id == id && f.IsNeedDeleteApprove);
            if (entity is null)
                return ResultResponse.Set(false, Resources.DataNotFound);

            entity.IsDeleted = true;
            entity.DeletedBy = CurrentUserGuid;
            entity.DeletedAt = DateTime.UtcNow;
            entity.IsNeedDeleteApprove = false;
            _context.Set<TEntity>().Update(entity);
            _context.SaveChanges();
            //_cacheProvider.Reset();
            return ResultResponse.Set(true, Resources.DataPublishedSuccessfully);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ResultResponse.Set(false, ex.Message);
        }
    }
    public ResultResponse CancelDelete(Guid id)
    {
        try
        {
            var entity = _context.Set<TEntity>().AsNoTracking().FirstOrDefault(f => f.Id == id && f.IsNeedDeleteApprove);
            if (entity is null)
                return ResultResponse.Set(false, Resources.DataNotFound);

            entity.IsDeleted = false;
            entity.DeletedBy = null;
            entity.DeletedAt = null;
            entity.IsNeedDeleteApprove = false;
            _context.Set<TEntity>().Update(entity);
            _context.SaveChanges();
            return ResultResponse.Set(true, Resources.DataPublishedSuccessfully);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ResultResponse.Set(false, ex.Message);
        }
    }
    public ResultResponse DeleteAll()
    {
        try
        {
            var entities = GetNeedDeleteRequestsGuids();
            foreach (var item in entities)
                ConfirmDelete(item);

            if (entities.Any())
            {
                //_cacheProvider.Reset();
                return ResultResponse.Set(true, Resources.DataDeletedSuccessfully);
            }
            else
                return ResultResponse.Set(false, Resources.DataNotFound);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ResultResponse.Set(false, ex.Message);
        }
    }
    public ResultResponse CancelDeleteAll()
    {
        try
        {
            var guids = GetNeedDeleteRequestsGuids();
            foreach (var item in guids)
                CancelDelete(item);

            return ResultResponse.Set(true, Resources.DataPublishedSuccessfully);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ResultResponse.Set(false, ex.Message);
        }
    }
    #endregion

    #region General
    public string GetName(Guid? id) => GetPublishById(id).GetName();
    #endregion

    #region Utilities
    internal async Task<(IQueryable<TEntity> Query, int Total)> GetQueryableAndTotalAsync(Expression<Func<TEntity, bool>>? predicate)
    {
        var query = _context.Set<TEntity>().AsNoTracking().AsQueryable().NullSafeWhere(predicate);
        var total = await query.NullSafeWhere(predicate).Where(w => w.IsDeleted == false).CountAsync();
        return (query, total);
    }
    internal IQueryable<BaseVersion>? GetBaseVersion()
    {
        var versionType = GetVersionType();
        if (versionType == null) return null;

        var setMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes);
        if (setMethod == null)
            throw new InvalidOperationException("DbContext.Set<T>() method not found.");

        var genericMethod = setMethod.MakeGenericMethod(versionType);
        var dbSetObj = genericMethod.Invoke(_context, null);

        if (dbSetObj is not IQueryable rawQueryable)
            return null;

        // Cast IQueryable<Derived> to IQueryable<BaseVersion>
        var castMethod = typeof(Queryable).GetMethod(nameof(Queryable.Cast))!
            .MakeGenericMethod(typeof(BaseVersion));
        var castedQueryable = castMethod.Invoke(null, new object[] { rawQueryable });

        return castedQueryable as IQueryable<BaseVersion>;
    }

    internal object? VersionModelDataSet()
    {
        var versionType = GetVersionType();
        if (versionType == null) return null;

        var setMethod = _context.GetType()
            .GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);

        if (setMethod == null) return null;
        var genericMethod = setMethod.MakeGenericMethod(versionType);
        return genericMethod.Invoke(_context, null); // return as object
    }

    internal void AddVersion(TDto dto)
    {
        var versionType = GetVersionType();
        if (versionType == null) return;

        var version = _dynamicMapper.Map(dto, dto.GetType(), versionType);
        if (version == null)
            throw new InvalidOperationException("Mapping failed. Check your dynamic mapper.");

        version.SetProperty("Id", Guid.NewGuid());
        version.SetProperty("MainVersionId", dto.Id);
        version.SetProperty("CreatedBy", CurrentUserGuid);
        version.SetProperty("CreatedAt", DateTime.UtcNow);
        version.SetProperty("IsPublished", _sessionProvider.HasPublishPermission);
        if (_sessionProvider.HasPublishPermission)
        {
            version.SetProperty("PublishedBy", CurrentUserGuid);
            version.SetProperty("PublishedAt", DateTime.UtcNow);
        }
        //var setMethod = _context.GetType().GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance)?.MakeGenericMethod(versionType);
        var setMethod = _context.GetType()
       .GetMethods()
       .First(m => m.Name == nameof(DbContext.Set) && m.IsGenericMethod && m.GetParameters().Length == 0)
       .MakeGenericMethod(versionType);


        var dbSet = setMethod?.Invoke(_context, null);
        var addMethod = dbSet?.GetType().GetMethod(nameof(DbContext.Add), new[] { versionType });
        addMethod?.Invoke(dbSet, new[] { version });

        _context.SaveChanges();
    }
    internal void UpdateVersion(BaseVersion model)
    {
        var versionType = GetVersionType();
        if (versionType == null) return;

        var dbSet = VersionModelDataSet();
        var updateMethod = dbSet?.GetType().GetMethod(nameof(DbContext.Update), new[] { versionType });
        updateMethod?.Invoke(dbSet, new[] { _dynamicMapper.Map(model, model.GetType(), versionType) });
    }
    internal void DeleteVersion(BaseVersion model)
    {
        var versionType = GetVersionType();
        if (versionType == null) return;

        model.IsDeleted = true;
        var dbSet = VersionModelDataSet();
        var updateMethod = dbSet?.GetType().GetMethod(nameof(DbContext.Update), new[] { versionType });
        updateMethod?.Invoke(dbSet, new[] { _dynamicMapper.Map(model, model.GetType(), versionType) });
    }
    internal void RemoveOldHistoryVersions(Guid entityId)
    {
        var versions = GetBaseVersion()?.AsNoTracking().Where(w => w.MainVersionId == entityId && w.IsDeleted == false).OrderByDescending(o => o.CreatedAt);
        if (versions != null && versions.Count() > 10)
        {
            foreach (var item in versions.Skip(10))
                DeleteVersion(item);

            _context.SaveChanges();
        }
    }
    internal int GetSort(int sort)
    {
        try
        {
            if (sort > 0) return sort;
            var maxSort = _context.Set<TEntity>().AsNoTracking().Max(m => m.Sort);
            return maxSort + 1;
        }
        catch
        {
            return 1;
        }
    }
    internal async Task<ResultResponse> UpdateIsDeletedColumnAsync(Guid id, bool value, string successMessage)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity is null)
                return ResultResponse.Set(false, Resources.DataNotFound);

            entity.IsDeleted = _sessionProvider.HasPublishPermission && value;
            entity.DeletedBy = CurrentUserGuid;
            entity.DeletedAt = DateTime.UtcNow;
            entity.IsNeedDeleteApprove = !_sessionProvider.HasPublishPermission;
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
            return ResultResponse.Set(true, successMessage);

        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ResultResponse.Set(false, ex.Message);
        }
    }
    internal Type? GetVersionType() => Type.GetType($"Domain.ModelVersions.{typeof(TEntity).Name}Version, Domain");
    #endregion
}