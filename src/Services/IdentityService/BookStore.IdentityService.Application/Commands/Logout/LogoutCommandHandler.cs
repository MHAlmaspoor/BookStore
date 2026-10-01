using System.Reflection.Metadata;
using System.Security.Authentication;
using BookStore.IdentityService.Application.Abstraction.Authentication;
using BookStore.IdentityService.Application.Abstraction.Persistance;

using MediatR;

namespace BookStore.IdentityService.Application.Command.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (refreshToken is null || refreshToken.IsRevoked || refreshToken.IsExpired)
            throw new InvalidCredentialException("Invalid refresh token");

        refreshToken.Revoke();

        await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
