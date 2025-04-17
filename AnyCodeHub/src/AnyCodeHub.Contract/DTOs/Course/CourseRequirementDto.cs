using System;

namespace AnyCodeHub.Contract.DTOs.Course
{
    /// <summary>
    /// DTO đại diện cho một yêu cầu (requirement) của khóa học
    /// </summary>
    public record CourseRequirementDto
    {
        /// <summary>
        /// Mô tả yêu cầu
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// ID của người tạo
        /// </summary>
        public Guid CreatedBy { get; init; }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public CourseRequirementDto() { }

        /// <summary>
        /// Constructor với các tham số
        /// </summary>
        /// <param name="description">Mô tả yêu cầu</param>
        /// <param name="createdBy">ID người tạo</param>
        public CourseRequirementDto(string description, Guid createdBy = default)
        {
            Description = description;
            CreatedBy = createdBy;
        }
    }
} 