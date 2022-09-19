using PayCoreFinalProject.Dto;

namespace PayCoreFinalProject.Base.Response;

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public BaseResponse(bool success)
    {
        Data = default;
        Success = success;
        Message = success ? "Success" : "Fault";

    }

    public BaseResponse(string message)
    {
        Data = default;
        Success = false;
        if (!string.IsNullOrWhiteSpace(message))
        {
            Message = message;
        }
    }

    public BaseResponse(T data)
    {
        Success = true;
        Message = "Success";
        Data = data;

    }
    
}