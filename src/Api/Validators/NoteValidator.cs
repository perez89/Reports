namespace Api.Validators;

public class NoteValidator : AbstractValidator<Models.Note>
{
    /// <summary>  
    /// Validator Note for Note  
    /// </summary>  
    public NoteValidator()
    {
        var authorMaxLength = 30;
        var contentMaxLength = 120;

        RuleFor(x => x.Author).MaximumLength(authorMaxLength).WithMessage($"The author name should not have more than {authorMaxLength} characters.");

        RuleFor(x => x.Content).MaximumLength(contentMaxLength).WithMessage($"The content should not have more than {contentMaxLength} characters.");
    }
}
