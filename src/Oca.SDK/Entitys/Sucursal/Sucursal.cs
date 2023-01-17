using System.Collections.Generic;

namespace Oca.SDK.Entitys{
    /// <summary>
    /// Sucursal de OCA. <br/>
    /// En realidad, "Sucursal" es un "Centro de imposición" en el webservice de OCA.
    /// </summary>
    public class Sucursal
    {
        public Sucursal() { }
        public int Id { get; set; }
        public string Sigla { get; set; }
        public string Descripcion { get; set; }
        public string Calle { get; set; }
        public int? Numero { get; set; }
        public string? Torre { get; set; }
        public string? Piso { get; set; }
        public string? Departamento { get; set; }
        public string Localidad { get; set; }
        public string CodigoPostalPrincipal { get; set; }
        public string Provincia { get; set; }
        public string Telefono { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string TipoAgencia { get; set; }
        public string HorarioAtencion { get; set; }
        /// <summary>
        /// Servicios que de la sucursal. Ver <see cref="TipoServicio"/> para más información.
        /// </summary>
        public List<Servicio> Servicios { get; set; }
        /// <summary>
        /// Codigos postales que acepta la sucursal. 
        /// Por defecto, la sucursal tiene un código principal, pero por cercania, puede atender a códigos cercanos.
        /// </summary>
        public List<string> CodigosPostalesQueAcepta { get; set; }
    }
}