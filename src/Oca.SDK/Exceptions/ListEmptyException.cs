using System;

namespace Oca.SDK.Exceptions
{
    public class ListEmptyException : Exception
    {
        public ListEmptyException() : base("No se pudo obtener la lista de datos.")
        {
        }
        public ListEmptyException(string message) : base(message)
        {
        }
    }
}