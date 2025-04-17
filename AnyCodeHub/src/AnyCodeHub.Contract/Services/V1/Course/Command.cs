using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.DTOs.Course;
using AnyCodeHub.Contract.Enumerations;
using System.Windows.Input;

namespace AnyCodeHub.Contract.Services.V1.Course;

public static class Command
{
    /// <summary>
    /// Command để tạo mới một khóa học
    /// </summary>
    public record CreateCourseCommand(
        string name, 
        string? description, 
        decimal price, 
        decimal? salePrice, 
        string? imageUrl, 
        string? videoUrl, 
        string? slug, 
        string status, 
        Guid authorId, 
        CourseLevel level, 
        int totalViews, 
        double totalDuration, 
        double rating, 
        Guid createdBy,
        List<SectionDto>? sections,
        List<CourseBenefitDto>? benefits,
        List<CourseTechnologyDto>? technologies,
        List<CourseCategoryDto>? categories,
        List<CourseQADto>? qAs,
        List<CourseRequirementDto>? requirements
    ) : ICommand<Response.CourseResponse> 
    { 
        /// <summary>
        /// Tạo command từ DTO
        /// </summary>
        /// <param name="dto">DTO chứa dữ liệu để tạo khóa học</param>
        /// <returns>Command tạo khóa học</returns>
        public static CreateCourseCommand FromDto(CreateCourseRequestDto dto)
        {
            return new CreateCourseCommand(
                dto.Name,
                dto.Description,
                dto.Price,
                dto.SalePrice,
                dto.ImageUrl,
                dto.VideoUrl,
                dto.Slug,
                dto.Status,
                dto.AuthorId,
                dto.Level,
                dto.TotalViews,
                dto.TotalDuration,
                dto.Rating,
                dto.CreatedBy,
                dto.Sections,
                dto.Benefits,
                dto.Technologies,
                dto.Categories,
                dto.QAs,
                dto.Requirements
            );
        }
    }

    /// <summary>
    /// Command để cập nhật một khóa học
    /// </summary>
    public record UpdateCourseCommand(
        Guid id, 
        string name, 
        string? description, 
        decimal price, 
        decimal? salePrice, 
        string? imageUrl, 
        string? videoUrl, 
        string? slug, 
        string status, 
        Guid authorId, 
        CourseLevel level, 
        int totalViews, 
        double totalDuration, 
        double rating, 
        Guid updatedBy
    ) : ICommand<Response.CourseResponse> { }

    /// <summary>
    /// Command để xóa một khóa học
    /// </summary>
    public record DeleteCourseCommand(
        Guid id, 
        Guid deletedBy
    ) : ICommand<bool> { }
}
