using Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Cqrs.Customer.UpdateAvatar;


public class UpdateAvatarCommand : IRequest<Result>
{
    public Guid CustomerId { get; set; }
    public string OldImageUrl { get; set; } = string.Empty;
    public IFormFile NewImage { get; set; } = null!;
}
