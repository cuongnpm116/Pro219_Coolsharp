using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.UpdateAvatar;

internal class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand, Result>
{
    private readonly ICustomerRepository _customerRepository;
    public UpdateAvatarCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<Result> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _customerRepository.UpdateUserAvatar(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
