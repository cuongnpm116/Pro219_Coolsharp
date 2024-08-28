namespace Domain.Enums;
public enum ResultStatus
{
    Ok = 200,
    Created = 201,
    NoContent = 204,
    Invalid = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    Error = 500,
    Unavailable = 503,
}
