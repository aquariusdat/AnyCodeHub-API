using System;

namespace AnyCodeHub.Contract.DTOs.Course
{
    /// <summary>
    /// DTO đại diện cho một lợi ích (benefit) của khóa học
    /// </summary>
    public record CourseBenefitDto
    {
        /// <summary>
        /// Mô tả lợi ích của khóa học
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// ID của người tạo
        /// </summary>
        public Guid CreatedBy { get; init; }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public CourseBenefitDto() { }

        /// <summary>
        /// Constructor với các tham số
        /// </summary>
        /// <param name="description">Mô tả lợi ích</param>
        /// <param name="createdBy">ID người tạo</param>
        public CourseBenefitDto(string description, Guid createdBy = default)
        {
            Description = description;
            CreatedBy = createdBy;
        }
    }
} 