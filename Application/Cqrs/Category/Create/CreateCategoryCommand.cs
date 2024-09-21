using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Category.Create;
public record CreateCategoryCommand(string Name) : IRequest<Result<bool>>;
