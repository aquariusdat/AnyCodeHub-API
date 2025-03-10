using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.Lesson;

public static class Command
{
    public record CreateLessonCommand(string title, string? description, string? videoUrl, string? pdfUrl, Guid sectionId, Guid? courseId, double duration, Guid createdBy) : ICommand<Response.LessonResponse> { }
    public record UpdateLessonCommand(Guid id, string title, string? description, string? videoUrl, string? pdfUrl, Guid sectionId, Guid? courseId, double duration, Guid updatedBy) : ICommand<Response.LessonResponse> { }
    public record DeleteLessonCommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}