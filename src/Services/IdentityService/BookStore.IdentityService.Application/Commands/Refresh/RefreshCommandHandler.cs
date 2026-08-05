 using BookStore.IdentityService.Application.Abstraction.Authentication;
using BookStore.IdentityService.Application.Abstraction.Persistance;
using BookStore.IdentityService.Application.Abstractions.Persistence;
using BookStore.IdentityService.Application.Command.Refresh;
using BookStore.IdentityService.Application.Contracts.Authentication;
using BookStore.IdentityService.Domain.Exceptions;
using MediatR;
 public sealed class RefreshCommandHandler : IRequestHandler<RefreshCommand, LoginResponse>
{
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenProvider _tokenProvider;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            ITokenProvider tokenProvider,
            IRefreshTokenGenerator refreshTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _tokenProvider = tokenProvider;
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken,cancellationToken);

            if(refreshToken is null)
                throw new DomainException("Invalid refresh token");

            if(refreshToken.IsRevoked)
                throw new DomainException("Refresh token has been revoked.");

            if(refreshToken.IsExpired)
                throw new DomainException("Refresh token is expired.");

            var user = await _refreshTokenRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);

            if(user is null)
                throw new DomainException("User not found");

            var accessToken = _tokenProvider.CreateAccessToken(user);

            var newRefreshToken = _refreshTokenGenerator.Generate(user.Id);

            refreshToken.Revoke(newRefreshToken.Token);

            user.AddRefreshToken(newRefreshToken.Token,newRefreshToken.ExpiresOnUtc,null,null);

            await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LoginResponse(user.Id.Value, accessToken.AccessToken, newRefreshToken.Token, accessToken.ExpireAt);
        }
}
