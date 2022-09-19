namespace PayCoreFinalProject.Base.Response;

public class ApplicationResult<T> : CommonApplicationResult
{
    public T Result { get; set; }
    public ApplicationResult(T data)
    {
        Result = data;
    }

    public ApplicationResult(string error)
    {
        ErrorMessage = error;
        Succceded = false;

    }
    
}

public class CommonApplicationResult
{
    public bool Succceded { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
}