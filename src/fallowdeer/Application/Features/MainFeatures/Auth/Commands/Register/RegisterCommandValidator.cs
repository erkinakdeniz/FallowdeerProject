using FluentValidation;

namespace Application.Features.MainFeatures.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.UserForRegisterDto.FirstName).NotEmpty().MinimumLength(3).MaximumLength(30).Matches(@"^[A-Za-zğüşıöçİĞÜŞÖÇ\s]*$")
            .WithMessage("'{PropertyName}' sadece harfler ve boşluklar içermelidir.").Must(x => BannedWords(x)).WithMessage("Yasaklı kelimeler kullanamayınız.");
        RuleFor(c => c.UserForRegisterDto.LastName).NotEmpty().MinimumLength(3).MaximumLength(30).Matches(@"^[A-Za-zğüşıöçİĞÜŞÖÇ\s]*$")
            .WithMessage("'{PropertyName}' sadece harfler ve boşluklar içermelidir.").Must(x => BannedWords(x)).WithMessage("Yasaklı kelimeler kullanamayınız.");
        RuleFor(c => c.UserForRegisterDto.Email).EmailAddress().Must(x => BannedWords(x)).WithMessage("Yasaklı kelimeler kullanamayınız.");
        RuleFor(c => c.UserForRegisterDto.Password).NotEmpty().MinimumLength(6).MaximumLength(16);
    }
    private bool BannedWords(string word)
    {
        string[] bannedWords = { "test", "admin", "demo", "script", "alert", "deneme" };
        foreach (string bw in bannedWords)
        {
            if (word.Contains(bw))
            {
                return false;
            }
        }
        return true;
    }
}
