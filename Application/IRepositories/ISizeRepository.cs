using Application.Cqrs.Size;
using Application.Cqrs.Size.Create;
using Application.Cqrs.Size.Get;
using Application.Cqrs.Size.Update;
using Application.ValueObjects.Pagination;
using Domain.Primitives;

namespace Application.IRepositories;
public interface ISizeRepository
{

    Task<Result<PaginationResponse<SizeVm>>> GetSizes(GetSizesQuery request);
    Task<Result<SizeVm>> GetSizeById(Guid id);
    Task<Result<bool>> AddSize(CreateSizeCommand request);
    Task<Result<bool>> UpdateSize(UpdateSizeCommand request);

}
