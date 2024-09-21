using Application.Cqrs.Color;
using Application.Cqrs.Color.Create;
using Application.Cqrs.Color.Get;
using Application.Cqrs.Color.GetForSelect;
using Application.Cqrs.Color.Update;
using Application.ValueObjects.Pagination;
using Domain.Primitives;

namespace Application.IRepositories;
public interface IColorRepository
{
    Task<Result<PaginationResponse<ColorVm>>> GetColors(GetColorsQuery request);
    Task<Result<ColorVm>> GetColorById(Guid id);
    Task<Result<bool>> AddColor(CreateColorCommand request);
    Task<Result<bool>> UpdateColor(UpdateColorCommand request);
    Task<IReadOnlyList<ColorForSelectVm>> GetColorForSelectVms();
}
