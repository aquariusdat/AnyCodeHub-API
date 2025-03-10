using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.UserCourse;

public static class Response
{
    public record UserCourseResponse
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public Guid CourseId { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public Guid CreatedBy { get; init; }
        public Guid? UpdatedBy { get; init; }
    }
}