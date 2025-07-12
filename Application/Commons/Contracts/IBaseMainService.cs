using Domain.Commons;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace Application.Commons.Contracts;

public interface IBaseMainService<TDto, TEntity> where TDto : BaseDto where TEntity : BaseEntity
{
    //Select Data
    IEnumerable<TDto> GetAllPublished();
    Task<IEnumerable<TDto>> GetAllPublishedAsync();
    PaginatedResult<TDto> GetAllPublished(int pageNumber = 1, int pageSize = 10);
    Task<PaginatedResult<TDto>> GetAllPublishedAsync(int pageNumber = 1, int pageSize = 10);
    IEnumerable<TDto> GetAll();
    Task<IEnumerable<TDto>> GetAllAsync();
    PaginatedResult<TDto> GetAll(int pageNumber = 1, int pageSize = 10);
    Task<PaginatedResult<TDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    IEnumerable<TDto> Search(Expression<Func<TEntity, bool>>? predicate);
    Task<IEnumerable<TDto>> SearchAsync(Expression<Func<TEntity, bool>>? predicate);
    PaginatedResult<TDto> Search(Expression<Func<TEntity, bool>>? predicate, int pageNumber = 1, int pageSize = 10);
    Task<PaginatedResult<TDto>> SearchAsync(Expression<Func<TEntity, bool>>? predicate, int pageNumber = 1, int pageSize = 10);
    IEnumerable<TDto> SearchLatest(Expression<Func<TEntity, bool>>? predicate);
    Task<IEnumerable<TDto>> SearchLatestAsync(Expression<Func<TEntity, bool>>? predicate);
    PaginatedResult<TDto> SearchLatest(Expression<Func<TEntity, bool>>? predicate, int pageNumber = 1, int pageSize = 10);
    Task<PaginatedResult<TDto>> SearchLatestAsync(Expression<Func<TEntity, bool>>? predicate, int pageNumber = 1, int pageSize = 10);
    // Select by Id
    TDto GetById(Guid? id);
    Task<TDto> GetByIdAsync(Guid? id);
    TDto GetPublishById(Guid? id);
    Task<TDto> GetPublishByIdAsync(Guid? id);

    //CRUD Operations
    ResultResponse Save(TDto model, bool isAutoSort = false);
    Task<ResultResponse> SaveAsync(TDto model, bool isAutoSort = false);
    ResultResponse Save(TDto model, ModelStateDictionary modelState);
    Task<ResultResponse> SaveAsync(TDto model, ModelStateDictionary modelState);
    //Task<ResultResponse> SaveAsync(List<TDto> models);
    Task<ResultResponse> DeleteAsync(Guid id);
    Task<ResultResponse> DeleteRangeAsync(IEnumerable<TEntity> entities);
    Task<ResultResponse> RestoreAsync(Guid id);
    Task<ResultResponse> ResortAsync(Guid[] ids);
    Task<ResultResponse> ResortAsync(Guid[] ids, int pageNumber, int pageSize);
    Task<int> FindAndReplaceAsync(string oldKeyword, string newKeyword);
    //Select Publish and Delete Requests and Guids
    List<Guid> GetNeedPublishRequestsGuids(Expression<Func<TEntity, bool>>? predicate);
    List<Guid> GetNeedPublishRequestsGuids();
    List<Guid> GetNeedDeleteRequestsGuids(Expression<Func<TEntity, bool>>? predicate);
    List<Guid> GetNeedDeleteRequestsGuids();
    IEnumerable<TDto> GetPublishAndDeleteRequests();
    IEnumerable<TDto> GetPublishAndDeleteRequests(Expression<Func<TEntity, bool>>? predicate);
    //Content Count
    long GetCount();
    long GetCount(Expression<Func<TEntity, bool>>? predicate);
    int GetPublishRequestCount();
    int GetDeleteRequestCount();
    int GetPublishAndDeleteRequestCount();

    //Content History
    List<EntityHistoryViewModel> HistoryVersions(Guid id);
    EntityHistoryDetailsViewModel HistoryDetails(Guid id);

    // Publish Content Records
    ResultResponse ConfirmPublish(Guid id);
    ResultResponse CancelPublish(Guid id);
    ResultResponse PublishAll();
    ResultResponse CancelPublishAll();

    // Delete Content Records
    ResultResponse ConfirmDelete(Guid id);
    ResultResponse CancelDelete(Guid id);
    ResultResponse DeleteAll();
    ResultResponse CancelDeleteAll();

    //General
    string GetName(Guid? id);
}
