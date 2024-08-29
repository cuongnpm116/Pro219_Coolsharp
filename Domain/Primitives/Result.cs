using Domain.Enums;

namespace Domain.Primitives;
public class Result
{
    public ResultStatus Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<ValidationError> Errors { get; set; } = [];
    public bool IsSuccess => Status is ResultStatus.Ok or ResultStatus.NoContent or ResultStatus.Created;
    public static Result Success() => new()
    {
        Status = ResultStatus.Ok
    };

    public static Result Success(string message) => new()
    {
        Status = ResultStatus.Ok,
        Message = message
    };

    public static Result Invalid(string message) => new()
    {
        Status = ResultStatus.Invalid,
        Message = message,
    };

    public static Result Error(string message) => new()
    {
        Status = ResultStatus.Error,
        Message = message
    };

    public static Result ValidationError(IEnumerable<ValidationError> errors) => new()
    {
        Status = ResultStatus.Invalid,
        Errors = errors
    };
}
