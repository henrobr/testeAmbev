namespace Ambev.DeveloperEvaluation.Domain.Common;

public class EntityInt32 : EntityId<int>
{
    public override bool IsUnassigned()
    {
        return Id == 0;
    }
}
