using FluentValidation;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.Get;

public class GetCategoryInputValidator : AbstractValidator<GetCategoryInput>
{
    public GetCategoryInputValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}