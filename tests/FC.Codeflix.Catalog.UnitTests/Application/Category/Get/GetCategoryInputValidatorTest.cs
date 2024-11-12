using FC.Codeflix.Catalog.Application.UseCases.Category.Get;
using FluentAssertions;
using FluentValidation;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.Get;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryInputValidatorTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryInputValidatorTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ValidationOk))]
    [Trait("Application", "GetCategoryInputValidation - UseCases")]
    public void ValidationOk()
    {
        // Given
        var categoryInput = new GetCategoryInput(Guid.NewGuid());
        var validator = new GetCategoryInputValidator();

        // When
        var result = validator.Validate(categoryInput);

        // Then
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
    [Trait("Application", "GetCategoryInputValidation - UseCases")]
    public void InvalidWhenEmptyGuidId()
    {
        // Given
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        var categoryInput = new GetCategoryInput(Guid.Empty);
        var validator = new GetCategoryInputValidator();

        // When
        var result = validator.Validate(categoryInput);

        // Then
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}