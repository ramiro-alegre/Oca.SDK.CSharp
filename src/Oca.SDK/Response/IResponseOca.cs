using System.Collections.Generic;

namespace Oca.SDK.Response
{
    public interface IResponseListOca<T>
    {
        bool Success { get; set; }
        List<T> Data { get; set; }
        string Message { get; set; }
    }
}