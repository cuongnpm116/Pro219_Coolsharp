using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Size.Get;
public class GetSizesQuery() : PaginationRequest, IRequest<Result<PaginationResponse<SizeVm>>>;
