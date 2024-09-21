using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Color.Get;
public class GetColorsQuery() : PaginationRequest, IRequest<Result<PaginationResponse<ColorVm>>>;