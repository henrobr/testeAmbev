using Ambev.DeveloperEvaluation.Application.Customers.Commands.Create;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Costumers.TestData;

/// <summary>
/// Provides test data for <see cref="CreateCustomerCommand"/> unit tests.
/// </summary>
public static class CreateCustomerCommandHandlerTestData
{
    /// <summary>
    /// Faker instance used to generate valid <see cref="CreateCustomerCommand"/> objects.
    /// </summary>
    private static readonly Faker<CreateCustomerCommand> createCustomerCommandFaker = new Faker<CreateCustomerCommand>()
        .CustomInstantiator(f => new CreateCustomerCommand(
            f.Person.FullName
        ));

    /// <summary>
    /// Generates a valid <see cref="CreateCustomerCommand"/> with realistic fake data.
    /// </summary>
    /// <returns>A valid command object.</returns>
    public static CreateCustomerCommand GenerateValidCommand()
    {
        return createCustomerCommandFaker.Generate();
    }

    /// <summary>
    /// Generates an invalid <see cref="CreateCustomerCommand"/> with empty values to trigger validation errors.
    /// </summary>
    /// <returns>An invalid command object.</returns>
    public static CreateCustomerCommand GenerateInvalidCommand()
    {
        return new CreateCustomerCommand(string.Empty);
    }
}
