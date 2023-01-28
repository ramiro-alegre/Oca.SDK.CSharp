namespace Oca.SDK.Entitys{
    public class Destinatario{
        /// <summary>
        /// Constructor utilizado en el caso de que se quiera utilizar el "Inicializador de objetos".
        /// </summary>
        public Destinatario(){}
        public Destinatario(string apellido, string nombre, string calle, int numero, string localidad, string provincia, int codigoPostal){
            this.Apellido = apellido;
            this.Nombre = nombre;
            this.Calle = calle;
            this.Numero = numero;
            this.Localidad = localidad;
            this.Provincia = provincia;
            this.CodigoPostal = codigoPostal;
        }
        /// <summary>
        /// Obligatorio.
        /// </summary>
        public string Apellido {get;set;}
        /// <summary>
        /// Obligatorio.
        /// </summary>
        public string Nombre {get;set;}
        /// <summary>
        /// Obligatorio. Nombre de la calle.
        public string Calle {get;set;}
        /// <summary>
        /// Obligatorio. Numero de la calle.
        /// </summary>
        public int Numero {get;set;}
        /// <summary>
        /// Opcional. Piso del domicilio.
        /// </summary>
        public string? Piso {get;set;}
        /// <summary>
        /// Opcional. Departamento del domicilio.
        /// </summary>
        public string? Depto {get;set;}
        /// <summary>
        /// Obligatorio. Localidad del domicilio.
        /// </summary>
        public string Localidad {get;set;}
        /// <summary>
        /// Obligatorio. Provincia del domicilio.
        /// </summary>
        public string Provincia {get;set;}
        /// <summary>
        /// Obligatorio. Codigo postal del domicilio.
        /// </summary>
        public int CodigoPostal {get;set;}
        /// <summary>
        /// Opcional. Telefono del destinatario. <br/>
        /// Ejemplo: 49569622
        /// </summary>
        public string? Telefono {get;set;}
        /// <summary>
        /// Opcional. Email del destinatario.
        /// </summary>
        public string? Email {get;set;}
        /// <summary>
        /// Obligatorio sólo para entrega en Sucursal. Corresponde al ID Centro Imposicion OCA. <br/>
        /// De forma fácil, cuando se envia este ID, el envio pasa a ser automaticamente a sucursal y NO a domicilio.
        /// </summary>
        public int IdCentroImposicionDestino {get;set;}
        /// <summary>
        /// Opcional. En caso de enviarlo, el sistema envía un SMS cuando está en la sucursal destino.
        public string? Celular {get;set;}
        /// <summary>
        /// Opcional. Observaciones del destinatario.
        /// </summary>
        public string? Observaciones {get;set;}
    }
}