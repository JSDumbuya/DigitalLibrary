namespace DigitalLibrary.API.Common;

//For operations that do return a value
public record Result<T>(bool IsSuccess, T? Value, ErrorType Type, string? Message)
{
    public static Result<T> Success(T value) => new(true, value, ErrorType.None, null);
    public static Result<T> Fail(ErrorType type, string message) => new(false, default, type, message);
}

//For operation that do not return a value
public record Result(bool IsSuccess, ErrorType Type, string? Message)
{
    public static Result Success() => new(true, ErrorType.None, null);
    public static Result Fail(ErrorType type, string message) => new(false, type, message);
}