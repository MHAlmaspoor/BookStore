using FluentValidation;

namespace BookStore.IdentityService.Application.Command.Logout;

public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x=>x.RefreshToken).NotEmpty();
    }
}
