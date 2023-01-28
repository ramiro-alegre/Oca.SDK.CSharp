using System.Collections.Generic;

namespace Oca.SDK.Entitys{
    public class OrdenRetiroDatos{
        public OrdenRetiroDatos(OrigenOca origenOca, List<EnvioPaquete> enviosPaquetes){
            this.OrigenOca = origenOca;
            this.EnviosPaquetes = enviosPaquetes;
        }
        /// <summary>
        /// Origen desde el cual se retira el paquete.
        /// </summary>
        /// <value></value>
        public OrigenOca OrigenOca {get;set;}
        /// <summary>
        /// Diferentes envíos de paquetes. <br/>
        /// Puede ser uno o más envíos.
        /// </summary>
        /// <value></value>
        public List<EnvioPaquete> EnviosPaquetes {get;set;}
    }
}