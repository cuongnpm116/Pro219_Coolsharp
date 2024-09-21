using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Category.CheckExist;
public record CheckExistCategoryNameQuery(string Name)
    : IRequest<Result<bool>>;
