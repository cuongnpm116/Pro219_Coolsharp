using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Staff.AddStaff;
internal sealed class AddStaffCommandHandler : IRequestHandler<AddStaffCommand, Result>
{
    private readonly IStaffRepository _staffRepository;

    public AddStaffCommandHandler(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    public async Task<Result> Handle(AddStaffCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _staffRepository.AddStaffAsync(request);
            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
