namespace Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.DTOs;

public sealed record SaleDto(
    int Id,
    int StatusId,
    string StatusName,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName,
    List<SaleItemDto> Items)
{
    public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
}
