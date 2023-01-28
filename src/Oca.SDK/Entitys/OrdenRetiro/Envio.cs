namespace Oca.SDK.Entitys{
    public class Envio {
        /// <summary>
        /// Constructor utilizado en el caso de que se quiera utilizar el "Inicializador de objetos".
        /// </summary>
        public Envio(){}
        public Envio(string idOperativa, string nroRemito){
            this.IdOperativa = idOperativa;
            this.NroRemito = nroRemito;
        }
        /// <summary>
        /// Obligatorio. Operativa dada por OCA para el envío.
        /// </summary>
        public string IdOperativa {get;set;}
        /// <summary>
        /// Obligatorio. Corresponde al remito del cliente.
        /// </summary>
        public string NroRemito {get;set;}
    }
}