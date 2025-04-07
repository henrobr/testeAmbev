namespace Ambev.DeveloperEvaluation.Domain.Common;

public abstract class EntityId<T>
{
    public virtual T Id { get; protected set; }

    public abstract bool IsUnassigned();

    public override string ToString()
    {
        return $"{GetType().Name}: [Id={Id}]";
    }
}
