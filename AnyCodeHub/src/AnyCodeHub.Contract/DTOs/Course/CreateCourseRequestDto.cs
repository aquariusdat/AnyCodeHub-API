using AnyCodeHub.Contract.Enumerations;
using System;
using System.Collections.Generic;

namespace AnyCodeHub.Contract.DTOs.Course
{
    /// <summary>
    /// DTO đại diện cho yêu cầu tạo khóa học mới, bao gồm tất cả thông tin liên quan
    /// </summary>
    public record CreateCourseRequestDto
    {
        /// <summary>
        /// Tên khóa học
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Mô tả khóa học
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Giá khóa học
        /// </summary>
        public decimal Price { get; init; }

        /// <summary>
        /// Giá khuyến mãi
        /// </summary>
        public decimal? SalePrice { get; init; }

        /// <summary>
        /// URL hình ảnh của khóa học
        /// </summary>
        public string? ImageUrl { get; init; }

        /// <summary>
        /// URL video giới thiệu khóa học
        /// </summary>
        public string? VideoUrl { get; init; }

        /// <summary>
        /// Slug của khóa học (dùng cho URL)
        /// </summary>
        public string? Slug { get; init; }

        /// <summary>
        /// Trạng thái của khóa học
        /// </summary>
        public string Status { get; init; }

        /// <summary>
        /// ID của tác giả khóa học
        /// </summary>
        public Guid AuthorId { get; init; }

        /// <summary>
        /// Cấp độ của khóa học
        /// </summary>
        public CourseLevel Level { get; init; }

        /// <summary>
        /// Tổng số lượt xem
        /// </summary>
        public int TotalViews { get; init; }

        /// <summary>
        /// Tổng thời lượng của khóa học
        /// </summary>
        public double TotalDuration { get; init; }

        /// <summary>
        /// Đánh giá trung bình của khóa học
        /// </summary>
        public double Rating { get; init; }

        /// <summary>
        /// ID của người tạo
        /// </summary>
        public Guid CreatedBy { get; init; }

        /// <summary>
        /// Danh sách các chương của khóa học
        /// </summary>
        public List<SectionDto>? Sections { get; init; }

        /// <summary>
        /// Danh sách các lợi ích của khóa học
        /// </summary>
        public List<CourseBenefitDto>? Benefits { get; init; }

        /// <summary>
        /// Danh sách các công nghệ liên quan đến khóa học
        /// </summary>
        public List<CourseTechnologyDto>? Technologies { get; init; }

        /// <summary>
        /// Danh sách các danh mục của khóa học
        /// </summary>
        public List<CourseCategoryDto>? Categories { get; init; }

        /// <summary>
        /// Danh sách các câu hỏi và trả lời của khóa học
        /// </summary>
        public List<CourseQADto>? QAs { get; init; }

        /// <summary>
        /// Danh sách các yêu cầu của khóa học
        /// </summary>
        public List<CourseRequirementDto>? Requirements { get; init; }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public CreateCourseRequestDto() { }

        /// <summary>
        /// Constructor với các tham số cơ bản
        /// </summary>
        public CreateCourseRequestDto(
            string name,
            string? description,
            decimal price,
            Guid authorId,
            CourseLevel level,
            Guid createdBy)
        {
            Name = name;
            Description = description;
            Price = price;
            AuthorId = authorId;
            Level = level;
            CreatedBy = createdBy;
            Status = "Active";
            TotalViews = 0;
            TotalDuration = 0;
            Rating = 0;
        }
    }
} 