using System.Collections.Generic;

namespace Oca.SDK.Response
{
    /// <summary>
    /// Respuesta generica para todos los metodos. <br/>
    /// Contiene un booleano que indica si la respuesta fue exitosa o no.<br/>
    /// Contiene un dato del tipo T, que va a ser lo que tenga la data en el caso de que Sucess sea True<br/>
    /// Contiene un mensaje con información. Este es bueno revisarlo cuando "Sucess" es false.<br/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseSingleOca<T> : IResponseSingleOca<T> where T : class
    {
        /// <summary>
        /// True si la respuesta fue exitosa, caso contrario, false. <br/>
        /// En caso de que sea false, revisar el mensaje para ver que paso.
        /// </summary>
        /// <value></value> 
        public bool Success { get; set; }
        /// <summary>
        /// Respuesta del tipo <see cref="T"/> <br/>
        /// En caso de que Success sea false, este campo será null.
        /// </summary>
        public T? Data { get; set; }
        /// <summary>
        /// Mensaje de información. <br/>
        /// En caso de que Success sea false, este mensaje contiene información sobre lo que sucedio.
        /// </summary>
        public string Message { get; set; }
    }
}