using Ambev.DeveloperEvaluation.Application.Products.Commands.Create;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.TestData;

/// <summary>
/// Provides test data for <see cref="CreateProductCommandHandler"/> unit tests.
/// </summary>
public static class CreateProductCommandHandlerTestData
{
    /// <summary>
    /// Faker instance used to generate valid <see cref="CreateProductCommand"/> objects.
    /// </summary>
    private static readonly Faker<CreateProductCommand> createProductCommandFaker = new Faker<CreateProductCommand>()
        .CustomInstantiator(f => new CreateProductCommand(
            f.Commerce.ProductName(),
            f.Random.Decimal(1, 1000)
        ));

    /// <summary>
    /// Generates a valid <see cref="CreateProductCommand"/> with realistic fake data.
    /// </summary>
    /// <returns>A valid command object.</returns>
    public static CreateProductCommand GenerateValidCommand()
    {
        return createProductCommandFaker.Generate();
    }

    /// <summary>
    /// Generates an invalid <see cref="CreateProductCommand"/> with empty and zero values to trigger validation errors.
    /// </summary>
    /// <returns>An invalid command object.</returns>
    public static CreateProductCommand GenerateInvalidCommand()
    {
        return new CreateProductCommand(string.Empty, 0);
    }
}
