using System.Collections.Generic;

namespace Oca.SDK.Response
{
    public interface IResponseSingleOca<T>
    {
        bool Success { get; set; }
        T Data { get; set; }
        string Message { get; set; }
    }
}