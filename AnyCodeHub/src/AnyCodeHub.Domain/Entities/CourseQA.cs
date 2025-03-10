using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.CourseQA;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseQA : AggregateRoot<Guid>, IBaseAuditEntity
{
    private CourseQA()
    {
    }

    private CourseQA(string question, string answer, Guid courseId)
    {
        Id = Guid.NewGuid();
        Question = question;
        Answer = answer;
        CourseId = courseId;
    }

    public static CourseQA Create(string question, string answer, Guid courseId)
    {
        CourseQA courseQA = new CourseQA(question, answer, courseId);
        courseQA.RaiseDomainEvent(new DomainEvent.CourseQACreated(Guid.NewGuid(), courseQA.Id, question, answer));
        return courseQA;
    }

    public void Update(Guid id, string question, string answer, Guid courseId)
    {
        Id = id;
        Question = question;
        Answer = answer;
        CourseId = courseId;

        RaiseDomainEvent(new DomainEvent.CourseQAUpdated(Guid.NewGuid(), Id, Question, Answer));
    }

    public string Question { get; private set; }
    public string Answer { get; private set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
    public Guid CourseId { get; private set; }
}
