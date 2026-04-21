namespace BookShop.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}