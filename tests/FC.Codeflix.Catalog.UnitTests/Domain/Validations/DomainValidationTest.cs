using Bogus;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Validations;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validations;

public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        // Given
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var value = Faker.Commerce.ProductName();

        // When
        var action = () => DomainValidation.NotNull(value, fieldName);

        // Then
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        // Given
        string? value = null;
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        
        // When
        var action = () => DomainValidation.NotNull(value, fieldName);
        
        // Then
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} cannot be null");
    }


    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        // Given
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        // When
        var action = () => DomainValidation.NotNullOrEmpty(target, fieldName);
        
        // Then
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} is required");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        // Given
        var target = Faker.Commerce.ProductName();
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        // When
        var action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

        // Then
        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        // Given
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        // When
        var action = () => DomainValidation.MinLength(target, minLength, fieldName);

        // Then
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} must be longer than {minLength} characters");
    }

    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
    public void MinLengthOk(string target, int minLength)
    {
        // Given
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        // When
        var action = () => DomainValidation.MinLength(target, minLength, fieldName);

        // Then
        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
    public void MaxLengthThrowWhenGreater(string target, int maxLength)
    {
        // Given
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        // When
        var action = () => DomainValidation.MaxLength(target, maxLength, fieldName);

        // Then
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} must be less than {maxLength} characters");
    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
    public void MaxLengthOk(string target, int maxLength)
    {
        // Given
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        // When
        var action = () => DomainValidation.MaxLength(target, maxLength, fieldName);

        // Then
        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOftests = 5)
    {
        yield return new object[] { "123456", 10 };
        var faker = new Faker();
        for(int i = 0; i < (numberOftests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + new Random().Next(1, 20);
            yield return new object[] { example, minLength };
        }
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOftests = 5)
    {
        yield return new object[] { "123456", 6 };
        var faker = new Faker();
        for (int i = 0; i < (numberOftests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length - new Random().Next(1, 5);
            yield return new object[] { example, minLength };
        }
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOftests = 5)
    {
        yield return new object[] { "123456", 5 };
        var faker = new Faker();
        for (int i = 0; i < (numberOftests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length - new Random().Next(1, 5);
            yield return new object[] { example, maxLength };
        }
    }

    public static IEnumerable<object[]> GetValuesLessThanMax(int numberOftests = 5)
    {
        yield return new object[] { "123456", 6 };
        var faker = new Faker();
        for (int i = 0; i < (numberOftests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length + new Random().Next(0, 5);
            yield return new object[] { example, maxLength };
        }
    }
}