using FluentValidation;
using AppCore.Dto;
using AppCore.Repositories;

public class CreatePersonDtoValidator : AbstractValidator<CreatePersonDto>
{
    private readonly ICompanyRepository _companyRepository;

    public CreatePersonDtoValidator(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;

        // Имя
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imię jest wymagane.")
            .MaximumLength(100).WithMessage("Imię za długie.");

        // Фамилия (то, что ты должна была дописать)
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .MaximumLength(200).WithMessage("Nazwisko nie może przekraczać 200 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Nazwisko zawiera niedozwolone znaki.");

        // Почта
        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress().WithMessage("Nieprawidłowy email.");

        // Телефон (регулярное выражение для формата)
        RuleFor(x => x.Phone)
            .Matches(@"^(\+?\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")
            .WithMessage("Nieprawidłowy format numeru telefonu.")
            .When(x => x.Phone is not null);

        // Возраст (минимум 18 лет)
        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Today.AddYears(-18))
            .WithMessage("Osoba musi mieć co najmniej 18 lat.");
    }
}