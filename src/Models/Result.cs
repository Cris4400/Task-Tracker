public class Result
{
    public bool State { get; init; }
    public string Message { get; init; } = string.Empty;
    public object? Data { get; init; }

    public static Result Success(string m, object? d)
    {
        return new Result { State = true, Message = m, Data = d};
    }

    public static Result Failure(string m, object? d)
    {
        return new Result { State = false, Message = m, Data = d};
    }
}