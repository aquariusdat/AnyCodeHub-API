using System;
using System.Collections.Generic;

namespace AnyCodeHub.Contract.DTOs.Course
{
    /// <summary>
    /// DTO đại diện cho một chương (section) trong khóa học
    /// </summary>
    public record SectionDto
    {
        /// <summary>
        /// Tên của chương
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Danh sách các bài học trong chương
        /// </summary>
        public List<LessonDto>? Lessons { get; init; }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public SectionDto() { }

        /// <summary>
        /// Constructor với các tham số
        /// </summary>
        /// <param name="name">Tên chương</param>
        /// <param name="lessons">Danh sách bài học</param>
        public SectionDto(string name, List<LessonDto>? lessons = null)
        {
            Name = name;
            Lessons = lessons;
        }
    }
} 