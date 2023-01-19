using System.Collections.Generic;

namespace Oca.SDK.Response
{
    public class ResponseOca<T> : IResponseOca<T>
    {
        public bool Success { get; set; }
        public List<T> Data { get; set; }
        public string Message { get; set; }
    }
}