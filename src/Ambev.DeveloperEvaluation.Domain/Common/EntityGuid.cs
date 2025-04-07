namespace Ambev.DeveloperEvaluation.Domain.Common;

public class EntityGuid : EntityId<Guid>
{
    public override bool IsUnassigned()
    {
        return Id == Guid.Empty;
    }
}
