// Abstractions/Result.cs
namespace AuthCourse.Abstractions;

public class Result<T>
{
    public T? Value { get; }
    public string? Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
        Error = null;
    }

    private Result(string error)
    {
        Error = error;
        IsSuccess = false;
        Value = default;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);
}

// Non-generic version for operations that return no value
public class Result
{
    public string? Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
}