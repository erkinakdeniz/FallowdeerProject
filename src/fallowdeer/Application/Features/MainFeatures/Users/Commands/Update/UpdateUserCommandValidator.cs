using FluentValidation;

namespace Application.Features.MainFeatures.Users.Commands.Update;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().MinimumLength(3).MaximumLength(30).Matches(@"^[A-Za-zğüşıöçİĞÜŞÖÇ\s]*$")
            .WithMessage("'{PropertyName}' sadece harfler ve boşluklar içermelidir.").Must(x => BannedWords(x)).WithMessage("Yasaklı kelimeler kullanamayınız.");
        RuleFor(c => c.LastName).NotEmpty().MinimumLength(3).MaximumLength(30).Matches(@"^[A-Za-zğüşıöçİĞÜŞÖÇ\s]*$")
            .WithMessage("'{PropertyName}' sadece harfler ve boşluklar içermelidir.").Must(x => BannedWords(x)).WithMessage("Yasaklı kelimeler kullanamayınız.");
        RuleFor(c => c.Email).EmailAddress();
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
