using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using static AnyCodeHub.Contract.Services.V1.Category.Query;
using static AnyCodeHub.Contract.Services.V1.Category.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Category;

public class GetCategoryQueryHandler : IQueryHandler<GetCategoryQuery, PagedResult<CategoryResponse>>
{
    private readonly ILogger<GetCategoryQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoryQueryHandler(
        ILogger<GetCategoryQueryHandler> logger,
        IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository,
        IMapper mapper)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<CategoryResponse>>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Build the query
            IQueryable<Domain.Entities.Category> query = _categoryRepository.FindAll(c => !c.IsDeleted);

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string searchTerm = request.SearchTerm.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(searchTerm) ||
                    (c.Description != null && c.Description.ToLower().Contains(searchTerm)));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortColumn) && request.SortOrder != null)
            {
                query = ApplySorting(query, request.SortColumn, request.SortOrder);
            }
            else if (request.SortColumnAndOrder != null && request.SortColumnAndOrder.Any())
            {
                foreach (var sort in request.SortColumnAndOrder)
                {
                    query = ApplySorting(query, sort.Key, sort.Value);
                }
            }
            else
            {
                // Default sorting by name
                query = query.OrderBy(c => c.Name);
            }

            // Calculate total count for pagination
            int totalCount = query.Count();

            // Apply pagination
            var pagedItems = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map to response DTOs
            var categoryResponses = pagedItems.Select(c => new CategoryResponse(
                c.Id,
                c.Name,
                c.Description,
                c.CreatedAt,
                c.UpdatedAt)).ToList();

            // Create paged result
            var pagedResult = PagedResult<CategoryResponse>.Create(
                categoryResponses,
                request.PageIndex,
                request.PageSize,
                totalCount);

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting categories: {ex.Message}");
            return Result.Failure<PagedResult<CategoryResponse>>(
                new Error("Category.QueryFailed", $"Failed to query categories: {ex.Message}"));
        }
    }

    private IQueryable<Domain.Entities.Category> ApplySorting(
        IQueryable<Domain.Entities.Category> query,
        string sortColumn,
        SortOrder sortOrder)
    {
        Expression<Func<Domain.Entities.Category, object>> keySelector = sortColumn.ToLower() switch
        {
            "name" => c => c.Name,
            "createdat" => c => c.CreatedAt,
            _ => c => c.Name // Default sort by name
        };

        return sortOrder == SortOrder.Ascending
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }
}