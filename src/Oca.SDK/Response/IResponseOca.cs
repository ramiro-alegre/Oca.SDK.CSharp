using System.Collections.Generic;

namespace Oca.SDK.Response
{
    public interface IResponseOca<T>
    {
        bool Success { get; set; }
        List<T> Data { get; set; }
        string Message { get; set; }
    }
}