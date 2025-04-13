/// <summary>
/// Class <c>Result</c> models the result of an operation with properties such as State, Message, and Data.
/// </summary>
public class Result
{
    public bool State { get; init; }
    public string Message { get; init; } = string.Empty;
    public object? Data { get; init; }

    /// <summary>
    /// Creates a new instance of the Result class with the specified state (always true), message, and data.
    /// <summary>
    /// <param name="message">The message associated with the result.</param>
    /// <param name="data">The data associated with the result (can be null).</param>
    /// <returns>A new instance of the Result class.</returns>
    public static Result Success(string m, object? d)
    {
        return new Result { State = true, Message = m, Data = d};
    }

    /// <summary>
    /// Creates a new instance of the Result class with the specified state (always false), message, and data.
    /// <param name="message">The message associated with the result.</param>
    /// <param name="data">The data associated with the result (can be null).</param>
    /// <returns>A new instance of the Result class.</returns>
    public static Result Failure(string m, object? d)
    {
        return new Result { State = false, Message = m, Data = d};
    }
}