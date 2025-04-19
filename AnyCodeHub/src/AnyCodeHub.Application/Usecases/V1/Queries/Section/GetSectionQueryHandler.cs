using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using static AnyCodeHub.Contract.Services.V1.Section.Query;
using static AnyCodeHub.Contract.Services.V1.Section.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Section;

public class GetSectionQueryHandler : IQueryHandler<GetSectionQuery, PagedResult<SectionResponse>>
{
    private readonly ILogger<GetSectionQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IMapper _mapper;

    public GetSectionQueryHandler(
        ILogger<GetSectionQueryHandler> logger,
        IRepositoryBase<Domain.Entities.Section, Guid> sectionRepository,
        IMapper mapper)
    {
        _logger = logger;
        _sectionRepository = sectionRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<SectionResponse>>> Handle(GetSectionQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Build the query
            IQueryable<Domain.Entities.Section> query = _sectionRepository.FindAll(s => !s.IsDeleted);

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string searchTerm = request.SearchTerm.ToLower();
                query = query.Where(s => s.Name.ToLower().Contains(searchTerm));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortColumn) && request.SortOrder != null)
            {
                query = ApplySorting(query, request.SortColumn, request.SortOrder.Value);
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
                query = query.OrderBy(s => s.Name);
            }

            // Calculate total count for pagination
            int totalCount = query.Count();

            // Apply pagination
            var pagedItems = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map to response DTOs
            var sectionResponses = _mapper.Map<List<SectionResponse>>(pagedItems);

            // Create paged result
            var pagedResult = PagedResult<SectionResponse>.Create(
                sectionResponses,
                request.PageIndex,
                request.PageSize,
                totalCount);

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting sections: {ex.Message}");
            return Result.Failure<PagedResult<SectionResponse>>(
                new Error("Section.QueryFailed", $"Failed to query sections: {ex.Message}"));
        }
    }

    private IQueryable<Domain.Entities.Section> ApplySorting(
        IQueryable<Domain.Entities.Section> query,
        string sortColumn,
        SortOrder sortOrder)
    {
        Expression<Func<Domain.Entities.Section, object>> keySelector = sortColumn.ToLower() switch
        {
            "name" => s => s.Name,
            "courseid" => s => s.CourseId,
            "createdat" => s => s.CreatedAt,
            _ => s => s.Name // Default sort by name
        };

        return sortOrder == SortOrder.Ascending
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }
}