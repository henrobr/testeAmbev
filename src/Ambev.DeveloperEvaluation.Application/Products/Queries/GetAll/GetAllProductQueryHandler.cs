using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.DTOs;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetAll;

/// <summary>
/// Handles the <see cref="GetAllProductQuery"/> to retrieve a list of products,
/// optionally filtered by name, without tracking changes.
/// </summary>
/// <param name="productRepository">The repository used to access product data.</param>
public sealed class GetAllProductQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetAllProductQuery, IQueryable<ProductDto>>
{
    /// <inheritdoc />
    public async Task<IQueryable<ProductDto>> Handle(GetAllProductQuery request,
        CancellationToken cancellationToken)
    {
        return productRepository.GetAllAsNoTracking(request.Name);
    }
}