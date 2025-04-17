using System;

namespace AnyCodeHub.Contract.DTOs.Course
{
    /// <summary>
    /// DTO đại diện cho một câu hỏi và trả lời (Q&A) của khóa học
    /// </summary>
    public record CourseQADto
    {
        /// <summary>
        /// Câu hỏi
        /// </summary>
        public string Question { get; init; }

        /// <summary>
        /// Câu trả lời
        /// </summary>
        public string Answer { get; init; }

        /// <summary>
        /// ID của người tạo
        /// </summary>
        public Guid CreatedBy { get; init; }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public CourseQADto() { }

        /// <summary>
        /// Constructor với các tham số
        /// </summary>
        /// <param name="question">Câu hỏi</param>
        /// <param name="answer">Câu trả lời</param>
        /// <param name="createdBy">ID người tạo</param>
        public CourseQADto(string question, string answer, Guid createdBy = default)
        {
            Question = question;
            Answer = answer;
            CreatedBy = createdBy;
        }
    }
} 