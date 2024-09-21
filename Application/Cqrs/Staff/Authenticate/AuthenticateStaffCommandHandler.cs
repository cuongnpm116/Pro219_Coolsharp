

using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Staff.Authenticate;

internal sealed class AuthenticateStaffCommandHandler : IRequestHandler<AuthenticateStaffCommand, Result>
{
    private readonly IStaffRepository _staffRepository;
    public AuthenticateStaffCommandHandler(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }
    public async Task<Result> Handle(AuthenticateStaffCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _staffRepository.Authenticate(request.Username, request.Password);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
