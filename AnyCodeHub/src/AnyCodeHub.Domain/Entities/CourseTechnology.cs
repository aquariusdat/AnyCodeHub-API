using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.CourseTechnology;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseTechnology : AggregateRoot<Guid>
{
    private CourseTechnology()
    {
    }

    private CourseTechnology(Guid courseId, Guid technologyId)
    {
        Id = Guid.NewGuid();
        CourseId = courseId;
        TechnologyId = technologyId;
    }

    public static CourseTechnology Create(Guid courseId, Guid technologyId)
    {
        CourseTechnology courseTechnology = new CourseTechnology(courseId, technologyId);
        courseTechnology.RaiseDomainEvent(new DomainEvent.CourseTechnologyCreated(Guid.NewGuid(), courseTechnology.Id, courseId, technologyId));
        return courseTechnology;
    }

    public void Update(Guid id, Guid courseId, Guid technologyId)
    {
        Id = id;
        CourseId = courseId;
        TechnologyId = technologyId;

        RaiseDomainEvent(new DomainEvent.CourseTechnologyUpdated(Guid.NewGuid(), Id, CourseId, TechnologyId));
    }

    public Guid CourseId { get; private set; }
    public Guid TechnologyId { get; private set; }
}
