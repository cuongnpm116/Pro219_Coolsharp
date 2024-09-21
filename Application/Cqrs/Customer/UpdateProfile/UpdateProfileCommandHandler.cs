using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.UpdateProfile;

internal sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly ICustomerRepository _customerRepository;
    public UpdateProfileCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _customerRepository.UpdatePersonalInformation(request, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
