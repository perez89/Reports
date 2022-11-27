namespace Api.Validators;

public class ReportValidator : AbstractValidator<Models.Report>
{
    /// <summary>  
    /// Validator Report for Note  
    /// </summary>  
    public ReportValidator()
    {
        var titleMaxLength = 30;
        var contentMaxLength = 120;

        RuleFor(x => x.Title).MaximumLength(titleMaxLength).WithMessage($"The title should not have more than {titleMaxLength} characters.");

        RuleFor(x => x.Content).MaximumLength(contentMaxLength).WithMessage($"The content should not have more than {contentMaxLength} characters.");
    }
}
