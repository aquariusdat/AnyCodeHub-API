using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.CourseBenefit;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseBenefit : AggregateRoot<Guid>
{
    private CourseBenefit()
    {
    }

    private CourseBenefit(string benefitContent)
    {
        Id = Guid.NewGuid();
        BenefitContent = benefitContent;
    }

    public static CourseBenefit Create(string benefitContent)
    {
        CourseBenefit benefit = new CourseBenefit(benefitContent);
        benefit.RaiseDomainEvent(new DomainEvent.CourseBenefitCreated(Guid.NewGuid(), benefit.Id, benefitContent));
        return benefit;
    }

    public void Update(Guid id, string benefitContent)
    {
        Id = id;
        BenefitContent = benefitContent;

        RaiseDomainEvent(new DomainEvent.CourseBenefitUpdated(Guid.NewGuid(), Id, benefitContent));
    }

    public string BenefitContent { get; private set; }
}
