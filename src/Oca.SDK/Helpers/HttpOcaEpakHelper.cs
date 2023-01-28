using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using Oca.SDK.Entitys;
using Oca.SDK.Exceptions;

namespace Oca.SDK.Services{
    internal class HttpOcaEpakHelper{
        private readonly string _url;
        public HttpOcaEpakHelper(string url){
            _url = url;
        }

        /// <summary>
        /// Recibe el DataSet de la respuesta de OCA de las sucursales, y la procesa para así obtener la lista de la misma.
        /// </summary>
        /// <param name="dataset">Xml de respuesta de Oca, parseado a un Dataset para trabajarlo con más facilidad</param>
        /// <param name="tipo">Filtro que se le quiera agregara a las sucursales</param>
        /// <param name="conCodigosPostalesAcepta">En el caso de que sea TRUE, rellena la lista "CodigosPostalesQueAcepta" de la clase Sucursal, caso contrario, la deja vacia y sin inicializar</param>
        /// <returns>Lista de sucursales</returns>
        public List<Sucursal> DataSetToSucursales(DataSet dataset, TipoServicio tipo, bool conCodigosPostalesAcepta = true)
        {
            List<Sucursal> sucursales = new List<Sucursal>();
            if(dataset.Tables.Count == 0)
                throw new ListEmptyException("No se pudo obtener la lista de sucursales.");
            for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
            {
                Sucursal sucursal = new Sucursal();
                DataRow[] serviciosSucursal = dataset.Tables[2].Select("Servicios_Id = " + i);
                
                if(tipo is TipoServicio.SinFiltro)
                {
                    sucursal = this.DataRowToSucursal(dataset.Tables[0].Rows[i], serviciosSucursal, conCodigosPostalesAcepta);
                    sucursales.Add(sucursal);
                } 
                else if(tipo is TipoServicio.AdmisionDePaquetes && ValidarSiEsTipoEnviado(serviciosSucursal, tipo))
                {
                    sucursal = this.DataRowToSucursal(dataset.Tables[0].Rows[i], serviciosSucursal, conCodigosPostalesAcepta);
                    sucursales.Add(sucursal);
                }
                else if (tipo is TipoServicio.EntregaDePaquetes && ValidarSiEsTipoEnviado(serviciosSucursal, tipo))
                {
                    sucursal = this.DataRowToSucursal(dataset.Tables[0].Rows[i], serviciosSucursal, conCodigosPostalesAcepta);
                    sucursales.Add(sucursal);
                }
                else if (tipo is TipoServicio.VentaEstampillas && ValidarSiEsTipoEnviado(serviciosSucursal, tipo))
                {
                    sucursal = this.DataRowToSucursal(dataset.Tables[0].Rows[i], serviciosSucursal, conCodigosPostalesAcepta);
                    sucursales.Add(sucursal);
                }
            }
            return sucursales;
        }
        /// <summary>
        /// Se encarga de crear una lista de <see cref="Provincia"/> a partir de un DataSet
        /// </summary>
        /// <param name="data">DataSet de la información de la provincia de oca</param>
        /// <returns>Lista de provincias creada a partir del DataSet enviado</returns>
        public List<Provincia> DataSetToProvincias(DataSet data)
        {
            List<Provincia> provincias = new List<Provincia>();
            if(data.Tables.Count == 0)
                throw new ListEmptyException("No se pudo obtener la lista de provincias");
            foreach (DataRow row in data.Tables[0].Rows)
            {
                provincias.Add(
                    new Provincia()
                    {
                        Id = Convert.ToInt32(row["IdProvincia"].ToString()),
                        Nombre = row["Descripcion"].ToString().Trim()
                    }
                );
            }
            return provincias;
        }
        /// <summary>
        /// Se encarga de crear una lista de <see cref="EstadoEnvio"/> a partir de un DataSet
        /// </summary>
        /// <param name="data">DataSet de la información de los estados del envio</param>
        /// <returns>Lista de estados a partir del DataSet enviado</returns>
        public List<EstadoEnvio> DataSetToEstado(DataSet data)
        {
            List<EstadoEnvio> estados = new List<EstadoEnvio>();
            if(data.Tables.Count == 0)
                throw new ListEmptyException("No se encontraron estados para el envio");
            foreach (DataRow row in data.Tables[0].Rows)
            {
                DateTime.TryParse(row["fecha"].ToString(), out DateTime fecha);

                estados.Add(
                    new EstadoEnvio()
                    {
                        Estado = row["Desdcripcion_Estado"].ToString(),
                        MotivoEstado = row["Descripcion_Motivo"].ToString(),
                        Sucursal = row["SUC"].ToString(),
                        fecha = fecha
                    }
                );
            }
            return estados;
        }
        /// <summary>
        /// Se encarga de crear una lista de string de los codigos postales a partir de un DataSet
        /// </summary>
        /// <param name="data">DataSet de los codigos postales</param>
        /// <returns>Lista de codigos postales</returns>
        public List<string> DataSetToCodigosPostales(DataSet data)
        {
            List<string> codigosPostales = new List<string>();
            if(data.Tables.Count == 0)
                throw new ListEmptyException("No se encontraron codigos postales para la sucursal");
            foreach (DataRow row in data.Tables[0].Rows)
            {
                codigosPostales.Add(row["CodigoPostal"].ToString());
            }
            return codigosPostales;
        }
        /// <summary>
        /// Se encargar de crear una lista de <see cref="OrdenRetiroResponse"/> a partir de un DataSet. <br/>
        /// A pesar de generar una lista, solo se devolverá un elemento ya que la respuesta de la API es un solo elemento.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<OrdenRetiroResponse> DataSetToOrdenRetiroResponse(DataSet data){
            if(data.Tables[0].TableName == "Error" || data.Tables[0].TableName == "Errores")
                throw new ListEmptyException(data.Tables[0].Rows[0]["Descripcion"].ToString());
            if(data.Tables[1].Rows.Count == 0)
                return new List<OrdenRetiroResponse>();

            return new List<OrdenRetiroResponse>(){
                new OrdenRetiroResponse{
                    IdOrdenRetiro = data.Tables[1].Rows[0]["OrdenRetiro"].ToString(),
                    NumeroEnvio = data.Tables[1].Rows[0]["NumeroEnvio"].ToString()
                }
            };
        }
        /// <summary>
        /// Transforma el <see cref="OrdenRetiroDatos"/> en un xml para ser enviado al webservice de OCA.
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="nroCuenta"></param>
        /// <returns>Xml</returns>
        public string OrdenRetiroDatosToXml(OrdenRetiroDatos datos, string nroCuenta){
            string xmlTemporal = @$"<?xml version=""1.0"" encoding=""iso-8859-1"" standalone=""yes""?> 
            <ROWS> 
                <cabecera ver=""2.0"" nrocuenta=""{nroCuenta}""/> 
                <origenes> 
                    <origen calle=""{datos.OrigenOca.Calle}"" nro=""{datos.OrigenOca.Numero}"" piso=""{datos.OrigenOca.Piso}"" depto=""{datos.OrigenOca.Depto}"" cp=""{datos.OrigenOca.CodigoPostal}"" localidad=""{datos.OrigenOca.Localidad}"" provincia=""{datos.OrigenOca.Provincia}"" contacto=""{datos.OrigenOca.Contacto}"" email=""{datos.OrigenOca.Email}"" solicitante=""{datos.OrigenOca.Solicitante}"" observaciones=""{datos.OrigenOca.Observaciones}"" centrocosto=""{datos.OrigenOca.CentroCosto}"" idfranjahoraria=""{(int)datos.OrigenOca.FranjaHoraria}"" idcentroimposicionorigen=""{datos.OrigenOca.IdCentroImposicionOrigen}"" fecha=""{datos.OrigenOca.Fecha}""> 
                        <envios>";

            foreach(var envio in datos.EnviosPaquetes){
                xmlTemporal += @$"<envio idoperativa=""{envio.Envio.IdOperativa}"" nroremito=""{envio.Envio.NroRemito}""> 
                            <destinatario apellido=""{envio.Destinatario.Apellido}"" nombre=""{envio.Destinatario.Nombre}"" calle=""{envio.Destinatario.Calle}"" nro=""{envio.Destinatario.Numero}"" piso=""{envio.Destinatario.Piso}"" depto=""{envio.Destinatario.Depto}"" localidad=""{envio.Destinatario.Localidad}"" provincia=""{envio.Destinatario.Provincia}"" cp=""{envio.Destinatario.CodigoPostal}"" telefono=""{envio.Destinatario.Telefono}"" email=""{envio.Destinatario.Email}"" idci=""{envio.Destinatario.IdCentroImposicionDestino}"" celular=""{envio.Destinatario.Celular}"" observaciones=""{envio.Destinatario.Observaciones}""/> 
                            <paquetes>"; 
               foreach(var paquete in envio.Paquetes){
                    xmlTemporal +=  @$"<paquete alto=""{paquete.Alto}"" ancho=""{paquete.Ancho}"" largo=""{paquete.Largo}"" peso=""{paquete.Peso}"" valor=""{paquete.Valor}"" cant=""{paquete.Cantidad}"" />";
               }
                xmlTemporal +=  "</paquetes></envio>";
            }
            
            xmlTemporal += "</envios> </origen> </origenes> </ROWS>";
            return xmlTemporal;
        }
        /// <summary>
        /// Se encarga de procesar un DataRow que tiene la información de una sucursal de Oca.
        /// </summary>
        /// <param name="row">DataRow de la información de la sucursal de oca</param>
        /// <param name="serviciosSucursal">Servicios que provee esa sucursal</param>
        /// <param name="conCodigosPostalesAcepta">En el caso de que sea TRUE, rellena la lista "CodigosPostalesQueAcepta" de la clase Sucursal, caso contrario, la deja vacia y sin inicializar</param>
        /// <returns>Sucursal creada a partir del datarow enviado</returns>
        private Sucursal DataRowToSucursal(DataRow row, DataRow[] serviciosSucursal, bool conCodigosPostalesAcepta = false)
        {
            List<Servicio> servicios = new List<Servicio>();
            for(int i = 0; i < serviciosSucursal.Length; i++)
            {
                Servicio servicio = new Servicio()
                {
                    Id = Convert.ToInt32(serviciosSucursal[i][0].ToString()),
                    Descripcion = serviciosSucursal[i][1].ToString()
                };
                servicios.Add(servicio);
            }

            int? numeroCalle = this.ObtenerNumeroDeCalle(row["Numero"].ToString());
            Sucursal sucursal = new Sucursal()
            {
                Id = Convert.ToInt32(row["IdCentroImposicion"].ToString()),
                Sigla = row["Sigla"].ToString(),
                Descripcion = row["Sucursal"].ToString(),
                Calle = row["Calle"].ToString(),
                Numero = numeroCalle,
                Torre = row["Torre"].ToString(),
                Piso = row["Piso"].ToString(),
                Departamento = row["Depto"].ToString(),
                Localidad = row["Localidad"].ToString(),
                CodigoPostalPrincipal = row["CodigoPostal"].ToString(),
                Provincia = row["Provincia"].ToString(),
                Telefono = row["Telefono"].ToString(),
                Latitud = Convert.ToDouble(row["Latitud"].ToString()),
                Longitud = Convert.ToDouble(row["Longitud"].ToString()),
                TipoAgencia = row["TipoAgencia"].ToString(),
                HorarioAtencion = row["HorarioAtencion"].ToString(),
                Servicios = servicios
            };

            if (conCodigosPostalesAcepta)
            {
                WebClient wc = new WebClient();
                string urlfinal =  $"{_url}/Oep_TrackEPak.asmx/GetCodigosPostalesXCentroImposicion?idCentroImposicion={sucursal.Id}";
                string xmlString = wc.DownloadString(urlfinal);
                DataSet dataSet = OCA.SDK.Utils.XmlUtils.ToDataSet(xmlString);
                List<string> codigosPostalesQueAcepta = new List<string>();
                for(int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    codigosPostalesQueAcepta.Add(dataSet.Tables[0].Rows[i]["CodigoPostal"].ToString());
                }
                sucursal.CodigosPostalesQueAcepta = codigosPostalesQueAcepta;
            }

            return sucursal;
        }

        /// <summary>
        /// Valida si los servicios de la sucursal tienen el TipoServicio enviado
        /// </summary>
        /// <param name="serviciosSucursal">Servicios de la sucursal</param>
        /// <param name="tipo">Tipo de servicio elegido</param>
        /// <returns>True si el TipoServicio esta dentro del array, caso contrario, false</returns>
        private bool ValidarSiEsTipoEnviado(DataRow[] serviciosSucursal, TipoServicio tipo)
        {
            for (int i = 0; i < serviciosSucursal.Length; i++)
            {
                int idTipo = Convert.ToInt32(serviciosSucursal[i][0].ToString());
                if(idTipo == (int) tipo) 
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Obtiene el numero de la calle de la sucursal. 
        /// Este metodo se creo debido a que en las respuestas hay casos donde se puede llegar a ver un tipo
        /// double. Incluso, hay casos donde la sucursal no tiene numero, y simplemente la respuesta es "S/N"
        /// </summary>
        /// <param name="Numero">Numero de la sucursal</param>
        /// <returns>En el caso de que haya podido parsear a un int, se devuelve el numero, caso contrario, devuelve null</returns>
        private int? ObtenerNumeroDeCalle(string Numero)
        {
            if (int.TryParse(Numero, out int numero))
                return numero;
            // Un ejemplo de un return con Convert.ToInt32 es cuando numero es 283,3 por ejemplo.
            if (double.TryParse(Numero, out double numeroD))
                return Convert.ToInt32(Math.Round(numeroD));
            // * Un ejemplo de un return null es cuando numero es "S/N".
            return null;
        }
    }
}