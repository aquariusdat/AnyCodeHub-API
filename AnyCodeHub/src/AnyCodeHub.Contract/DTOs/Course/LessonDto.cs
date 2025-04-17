using System;

namespace AnyCodeHub.Contract.DTOs.Course
{
    /// <summary>
    /// DTO đại diện cho một bài học (lesson) trong khóa học
    /// </summary>
    public record LessonDto
    {
        /// <summary>
        /// Tiêu đề của bài học
        /// </summary>
        public string Title { get; init; }

        /// <summary>
        /// Mô tả của bài học
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Thời lượng của bài học (tính bằng phút)
        /// </summary>
        public double Duration { get; init; }

        /// <summary>
        /// URL của video bài học
        /// </summary>
        public string? VideoUrl { get; init; }

        /// <summary>
        /// URL của tài liệu đính kèm
        /// </summary>
        public string? AttachmentUrl { get; init; }

        /// <summary>
        /// ID của người tạo bài học
        /// </summary>
        public Guid CreatedBy { get; init; }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public LessonDto() { }

        /// <summary>
        /// Constructor với các tham số
        /// </summary>
        /// <param name="title">Tiêu đề bài học</param>
        /// <param name="description">Mô tả bài học</param>
        /// <param name="duration">Thời lượng bài học</param>
        /// <param name="videoUrl">URL video bài học</param>
        /// <param name="attachmentUrl">URL tài liệu đính kèm</param>
        /// <param name="createdBy">ID người tạo</param>
        public LessonDto(
            string title, 
            string? description = null, 
            double duration = 0, 
            string? videoUrl = null, 
            string? attachmentUrl = null,
            Guid createdBy = default)
        {
            Title = title;
            Description = description;
            Duration = duration;
            VideoUrl = videoUrl;
            AttachmentUrl = attachmentUrl;
            CreatedBy = createdBy;
        }
    }
} 