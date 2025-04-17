using System;

namespace AnyCodeHub.Contract.DTOs.Course
{
    /// <summary>
    /// DTO đại diện cho mối quan hệ giữa khóa học và công nghệ
    /// </summary>
    public record CourseTechnologyDto
    {
        /// <summary>
        /// ID của công nghệ
        /// </summary>
        public Guid TechnologyId { get; init; }

        /// <summary>
        /// ID của người tạo
        /// </summary>
        public Guid CreatedBy { get; init; }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public CourseTechnologyDto() { }

        /// <summary>
        /// Constructor với các tham số
        /// </summary>
        /// <param name="technologyId">ID của công nghệ</param>
        /// <param name="createdBy">ID người tạo</param>
        public CourseTechnologyDto(Guid technologyId, Guid createdBy = default)
        {
            TechnologyId = technologyId;
            CreatedBy = createdBy;
        }
    }
} 