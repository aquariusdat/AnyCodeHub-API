using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Category;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Category.Command;
using static AnyCodeHub.Contract.Services.V1.Category.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Category;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryResponse>
{
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(
        ILogger<UpdateCategoryCommandHandler> logger,
        IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository,
        IMapper mapper)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the category exists
            var category = await _categoryRepository.FindByIdAsync(request.id, cancellationToken);
            if (category == null || category.IsDeleted)
            {
                return Result.Failure<CategoryResponse>(
                    new Error("Category.NotFound", $"Category with ID {request.id} not found."));
            }

            // Check if the new name is already used by another category
            if (category.Name != request.name)
            {
                var existingCategory = _categoryRepository.FindAll(c =>
                    c.Name == request.name &&
                    c.Id != request.id &&
                    !c.IsDeleted).FirstOrDefault();

                if (existingCategory != null)
                {
                    return Result.Failure<CategoryResponse>(
                        new Error("Category.NameAlreadyExists", $"A category with the name '{request.name}' already exists."));
                }
            }

            // Update the category
            category.Update(
                request.id,
                request.name,
                request.description,
                request.updatedBy);

            // Update in repository
            _categoryRepository.Update(category);

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
            _logger.LogError($"Error while updating category: {ex.Message}");
            return Result.Failure<CategoryResponse>(
                new Error("Category.UpdateFailed", $"Failed to update category: {ex.Message}"));
        }
    }
}