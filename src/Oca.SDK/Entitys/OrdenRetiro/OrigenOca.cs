using System;
using System.Data.Common;
using Oca.SDK.Enums;

namespace Oca.SDK.Entitys{
    public class OrigenOca{
        /// <summary>
        /// Constructor utilizado en el caso de que se quiera utilizar el "Inicializador de objetos".
        /// </summary>
        public OrigenOca(){
            
        }
        public OrigenOca(string calle, int numero, string? piso, string? depto, int codigoPostal, string localidad, string provincia, string? contacto, string email, string? solicitante, string? observaciones, string centroCosto, FrajaHoraria franjaHoraria, int? idCentroImposicionOrigen, DateTime fecha){
            this.Calle = calle;
            this.Numero = numero;
            this.Piso = piso;
            this.Depto = depto;
            this.CodigoPostal = codigoPostal;
            this.Localidad = localidad;
            this.Provincia = provincia;
            this.Contacto = contacto;
            this.Email = email;
            this.Solicitante = solicitante;
            this.Observaciones = observaciones;
            this.CentroCosto = centroCosto;
            this.FranjaHoraria = franjaHoraria;
            this.IdCentroImposicionOrigen = idCentroImposicionOrigen;
            this.Fecha = fecha;
        }
        /// <summary>
        /// Obligatorio. 
        /// </summary>
        /// <value></value>
        public string Calle {get;set;}
        /// <summary>
        /// Obligatorio. Numero de la calle.
        /// </summary>
        public int Numero {get;set;}
        /// <summary>
        /// Opcional. Piso del domicilio.
        /// </summary>
        /// <value></value>
        public string? Piso {get;set;}
        /// <summary>
        /// Opcional. Departamento del domicilio.
        /// </summary>
        public string? Depto {get;set;}
        /// <summary>
        /// Obligatorio. Codigo postal del domicilio.
        /// </summary>
        public int CodigoPostal {get;set;}
        /// <summary>
        /// Obligatorio. Localidad del domicilio.
        /// </summary>
        public string Localidad {get;set;}
        /// <summary>
        /// Obligatorio. Provincia del domicilio.
        /// </summary>
        public string Provincia {get;set;}
        /// <summary>
        /// Opcional. Forma de contacto, puede ser un telefono o un email.
        /// </summary>
        public string? Contacto {get;set;}
        /// <summary>
        /// Obligatorio. Email de contacto.
        /// </summary>
        public string Email {get;set;}
        /// <summary>
        /// Opcional. No hay información acerca de este campo.
        /// </summary>
        public string? Solicitante{get;set;}
        /// <summary>
        /// Opcional.
        /// </summary>
        public string? Observaciones {get;set;}
        /// <summary>
        /// Obligatorio, corresponde al número de centro de costo habilitado en OCA para las sucursales del cliente habilitados en la operativa.
        /// </summary>
        public string CentroCosto {get;set;}
        /// <summary>
        /// Obligatorio. Franja horaria en la que se desea retirar el paquete.
        /// </summary>
        public FrajaHoraria FranjaHoraria {get;set;}
        /// <summary>
        /// Obligatorio sólo para Admisión en Sucursal. Corresponde al ID Centro Imposicion OCA que va admitir el envío.
        /// </summary>
        public int? IdCentroImposicionOrigen{get;set;}
        /// <summary>
        /// Obligatorio. Corresponde a la fecha de Admisión o Retiro según Operativa. Formato “AAAAMMDD”
        /// </summary>
        public DateTime Fecha {get;set;}
    }
}