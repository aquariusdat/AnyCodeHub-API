using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Category;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Category.Command;
using static AnyCodeHub.Contract.Services.V1.Category.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Category;

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly ILogger<CreateCategoryCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(
        ILogger<CreateCategoryCommandHandler> logger,
        IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository,
        IMapper mapper)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if a category with the same name already exists
            var existingCategory = _categoryRepository.FindAll(c => c.Name == request.name && !c.IsDeleted).FirstOrDefault();
            if (existingCategory != null)
            {
                return Result.Failure<CategoryResponse>(
                    new Error("Category.NameAlreadyExists", $"A category with the name '{request.name}' already exists."));
            }

            // Create the category
            var category = Domain.Entities.Category.Create(
                request.name,
                request.description,
                request.createdBy);

            // Add to repository
            _categoryRepository.Add(category);

            // Create response
            var response = new CategoryResponse(
                category.Id,
                category.Name,
                category.Description,
                category.CreatedAt,
                category.UpdatedAt);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while creating category: {ex.Message}");
            return Result.Failure<CategoryResponse>(
                new Error("Category.CreateFailed", $"Failed to create category: {ex.Message}"));
        }
    }
}