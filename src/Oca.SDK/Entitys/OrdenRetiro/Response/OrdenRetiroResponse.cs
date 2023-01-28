namespace Oca.SDK.Entitys{
    public class OrdenRetiroResponse{
        /// <summary>
        /// Numero de orden de retiro
        /// </summary>
        public string IdOrdenRetiro {get;set;}
        /// <summary>
        /// Numero de envio, este es que se utiliza para consultar en la página de OCA el envio.
        /// </summary>
        public string NumeroEnvio {get;set;}
    }
}