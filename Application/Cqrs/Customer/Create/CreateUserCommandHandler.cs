using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.Create
{
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly ICustomerRepository _customerRepository;
        public CreateUserCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _customerRepository.AddUser(request);
                return result;
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
