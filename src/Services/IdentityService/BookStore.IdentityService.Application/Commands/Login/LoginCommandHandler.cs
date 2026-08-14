using BookStore.IdentityService.Application.Abstraction.Authentication;
using BookStore.IdentityService.Application.Abstraction.Persistance;
using BookStore.IdentityService.Application.Abstractions.Authorization;
using BookStore.IdentityService.Application.Abstractions.Persistence;
using BookStore.IdentityService.Application.Abstractions.Security;
using BookStore.IdentityService.Application.Contracts.Authentication;
using BookStore.IdentityService.Domain.Exceptions;
using BookStore.IdentityService.Domain.ValueObjects;
using MediatR;

namespace BookStore.IdentityService.Application.Command.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserPermissionRepository _userPermissionRepository;

    public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenProvider tokenProvider,
        IRefreshTokenGenerator refreshTokenGenerator, IRefreshTokenRepository refreshTokenRepository, IUserPermissionRepository userPermissionRepository, IUnitOfWork unitOfWork)
    {
        _userRepository=userRepository;
        _passwordHasher=passwordHasher;
        _tokenProvider=tokenProvider;
        _refreshTokenGenerator=refreshTokenGenerator;
        _refreshTokenRepository=refreshTokenRepository;
        _userPermissionRepository = userPermissionRepository;
        _unitOfWork=unitOfWork;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);
        var user = await _userRepository.GetByEmailAsync(email,cancellationToken);
        var permissions = await _userPermissionRepository.GetPermissionsAsync(user.Id, cancellationToken);
        

        if(user is null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new DomainException("Invalid email or password.");

        var jwt = _tokenProvider.CreateAccessToken(user, permissions);

        var refreshToken = _refreshTokenGenerator.Generate(user.Id, request.Device, request.IpAddress);

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);


        return new LoginResponse(user.Id.Value, jwt.AccessToken, refreshToken.Token, jwt.ExpireAt);
    }
}
