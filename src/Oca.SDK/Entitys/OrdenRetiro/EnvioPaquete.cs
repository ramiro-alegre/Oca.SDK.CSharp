using System.Collections.Generic;

namespace Oca.SDK.Entitys{
    public class EnvioPaquete{
        /// <summary>
        /// Constructor utilizado en el caso de que se quiera utilizar el "Inicializador de objetos".
        /// </summary>
        public EnvioPaquete(){}
        public EnvioPaquete(Envio envio, Destinatario destinatario, List<Paquete> paquetes){
            this.Envio = envio;
            this.Destinatario = destinatario;
            this.Paquetes = paquetes;
        }
        /// <summary>
        /// Información para el envío. <br/>
        /// Se incluye el IdOperativa que define qué forma de envío se utilizará.
        /// </summary>
        /// <value></value>
        public Envio Envio {get;set;}
        /// <summary>
        /// Información del destinatario.
        /// </summary>
        /// <value></value>
        public Destinatario Destinatario {get;set;}
        /// <summary>
        /// Lista de paquetes a enviar. <br/>
        /// Puede ser uno o más paquetes.
        /// </summary>
        public List<Paquete> Paquetes {get;set;}
    }
}