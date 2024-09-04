

using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Voucher.GetVoucherById;

public class GetVoucherByIdQuery : IRequest<Result>
{
    public Guid Id { get; set; }
}
