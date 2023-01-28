namespace Oca.SDK.Entitys{
    public class Paquete{
        /// <summary>
        /// Constructor utilizado en el caso de que se quiera utilizar el "Inicializador de objetos".
        /// </summary>
        public Paquete(){}
        public Paquete(double alto, double ancho, double largo, double peso, double? valor, int cantidad){
            this.Alto = alto;
            this.Ancho = ancho;
            this.Largo = largo;
            this.Peso = peso;
            this.Valor = valor;
            this.Cantidad = cantidad;
        }
        /// <summary>
        /// Obligatorio. Alto del paquete en centímetros.
        /// </summary>
        public double Alto {get;set;}
        /// <summary>
        /// Obligatorio. Ancho del paquete en centímetros.
        /// </summary>
        public double Ancho {get;set;}
        /// <summary>
        /// Obligatorio. Largo del paquete en centímetros.
        /// </summary>
        public double Largo {get;set;}
        /// <summary>
        /// Obligatorio. Peso del paquete en kilogramos.
        /// </summary>
        public double Peso {get;set;}
        /// <summary>
        /// Obligatorio sólo para operativas con Seguro OCA, para el resto debe ingresar en 0. <br/>
        /// Valor declarado del paquete.
        /// </summary>
        public double? Valor {get;set;}
        /// <summary>
        /// Obligatorio. Cantidad de paquetes con las mismas caracteristicas <br/>
        /// Hay que tener en cuenta que esto genera tantas etiquetas como cantidad se ingrese, por lo tanto,
        /// es recomendable calcular los valores de todo lo que se vaya a enviar y dejar la cantidad en 1(Si es lo que se quiere). <br/>
        /// Esto siempre y cuando no superer los valores máximos permitidos por OCA.
        /// </summary>
        public int Cantidad {get;set;}
    }
}