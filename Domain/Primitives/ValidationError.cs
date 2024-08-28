namespace Domain.Primitives;
public record ValidationError(string PropertyName, string ErrorMessage);
