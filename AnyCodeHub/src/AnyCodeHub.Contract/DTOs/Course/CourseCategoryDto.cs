using System;

namespace AnyCodeHub.Contract.DTOs.Course
{
    /// <summary>
    /// DTO đại diện cho mối quan hệ giữa khóa học và danh mục
    /// </summary>
    public record CourseCategoryDto
    {
        /// <summary>
        /// ID của danh mục
        /// </summary>
        public Guid CategoryId { get; init; }

        /// <summary>
        /// ID của người tạo
        /// </summary>
        public Guid CreatedBy { get; init; }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public CourseCategoryDto() { }

        /// <summary>
        /// Constructor với các tham số
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <param name="createdBy">ID người tạo</param>
        public CourseCategoryDto(Guid categoryId, Guid createdBy = default)
        {
            CategoryId = categoryId;
            CreatedBy = createdBy;
        }
    }
} 