namespace WebAppIntegrated.ApiResponse;
public readonly record struct ValidationError(string PropertyName, string ErrorMessage);
