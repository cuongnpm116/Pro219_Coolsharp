using Domain.Enums;

namespace Domain.Primitives;
public class Result<T> : Result
{
    public T Value { get; set; } = default;

    public static Result<T> Success(T value) => new()
    {
        Status = ResultStatus.Ok,
        Value = value
    };

    public static new Result<T> Invalid(string message) => new()
    {
        Status = ResultStatus.Invalid,
        Message = message
    };

    public static new Result<T> Error(string message) => new()
    {
        Status = ResultStatus.Error,
        Message = message
    };
}
