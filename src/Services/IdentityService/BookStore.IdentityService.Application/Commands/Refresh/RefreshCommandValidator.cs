
using FluentValidation;

namespace BookStore.IdentityService.Application.Command.Refresh;
public sealed class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(x=> x.RefreshToken).NotEmpty();
    }


}
