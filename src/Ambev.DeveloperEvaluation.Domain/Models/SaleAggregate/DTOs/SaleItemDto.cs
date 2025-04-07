namespace Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.DTOs;

public sealed record SaleItemDto(
    int Id,
    int SaleId,
    int ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Discount)
{
    public decimal TotalPrice => (UnitPrice * Quantity) - Discount;
}