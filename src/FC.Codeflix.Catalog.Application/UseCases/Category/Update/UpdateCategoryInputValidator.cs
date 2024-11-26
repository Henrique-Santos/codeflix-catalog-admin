using FluentValidation;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.Update;

public class UpdateCategoryInputValidator : AbstractValidator<UpdateCategoryInput>
{
    public UpdateCategoryInputValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}