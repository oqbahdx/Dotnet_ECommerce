using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common;

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public string? Error { get; set; }

    public static BaseResponse<T> Ok(T data, string message = "") =>
        new() { Success = true, Data = data, Message = message };

    public static BaseResponse<T> Fail(string error, string message = "") =>
        new() { Success = false, Error = error, Message = message };
}
