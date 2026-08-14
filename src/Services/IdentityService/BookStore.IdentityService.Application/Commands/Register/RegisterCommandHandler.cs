using BookStore.IdentityService.Application.Abstractions.Persistence;
using BookStore.IdentityService.Application.Abstractions.Security;
using BookStore.IdentityService.Application.Abstraction.Persistance;
using BookStore.IdentityService.Domain.Common;
using BookStore.IdentityService.Domain.Exceptions;
using BookStore.IdentityService.Domain.Users;
using BookStore.IdentityService.Domain.ValueObjects;
using MediatR;

namespace BookStore.IdentityService.Application.Command.Register;

public sealed class RegisterCommandHandler:IRequestHandler<RegisterCommand, Guid>
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(IUserRepository repository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);

        if(await _repository.ExistsAsync(email ,cancellationToken))
            throw new DomainException(DomainErrors.User.EmailAlreadyExists);

        var passwordHash= _passwordHasher.Hash(request.Password);

        var user = new User(
            UserId.New(),
            email,
            passwordHash,
            request.FirstName,
            request.LastName);

        await _repository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
