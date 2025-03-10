using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.CourseQA;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseQA : AggregateRoot<Guid>
{
    private CourseQA()
    {
    }

    private CourseQA(string question, string answer)
    {
        Id = Guid.NewGuid();
        Question = question;
        Answer = answer;
    }

    public static CourseQA Create(string question, string answer)
    {
        CourseQA courseQA = new CourseQA(question, answer);
        courseQA.RaiseDomainEvent(new DomainEvent.CourseQACreated(Guid.NewGuid(), courseQA.Id, question, answer));
        return courseQA;
    }

    public void Update(Guid id, string question, string answer)
    {
        Id = id;
        Question = question;
        Answer = answer;

        RaiseDomainEvent(new DomainEvent.CourseQAUpdated(Guid.NewGuid(), Id, Question, Answer));
    }

    public string Question { get; private set; }
    public string Answer { get; private set; }
}
