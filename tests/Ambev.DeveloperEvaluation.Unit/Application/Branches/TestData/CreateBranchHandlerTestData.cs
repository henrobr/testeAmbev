using Ambev.DeveloperEvaluation.Application.Branches.Commands.Create;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.TestData;

/// <summary>
/// Provides test data for <see cref="CreateBranchCommand"/> unit tests.
/// </summary>
public static class CreateBranchHandlerTestData
{
    /// <summary>
    /// Faker instance used to generate valid <see cref="CreateBranchCommand"/> objects.
    /// </summary>
    private static readonly Faker<CreateBranchCommand> createBranchFaker = new Faker<CreateBranchCommand>()
        .CustomInstantiator(f => new CreateBranchCommand(
            f.Company.CompanyName()
        ));

    /// <summary>
    /// Generates a valid <see cref="CreateBranchCommand"/> with realistic fake data.
    /// </summary>
    /// <returns>A valid command object.</returns>
    public static CreateBranchCommand GenerateValidCommand()
    {
        return createBranchFaker.Generate();
    }

    /// <summary>
    /// Generates an invalid <see cref="CreateBranchCommand"/> with empty values to trigger validation errors.
    /// </summary>
    /// <returns>An invalid command object.</returns>
    public static CreateBranchCommand GenerateInvalidCommand()
    {
        return new CreateBranchCommand(string.Empty);
    }
}
