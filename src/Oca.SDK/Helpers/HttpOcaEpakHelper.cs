using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using Oca.SDK.Entitys;

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
        /// <returns>En el casod de que haya podido parsear a un int, se devuelve el numero, caso contrario, devuelve null</returns>
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

        /// <summary>
        /// Se encarga de crear un objeto Provincia a partir de un Dat
        /// </summary>
        /// <param name="row">DataRow de la información de la provincia de oca</param>
        /// <returns>Provincia creada a partir del datarow enviado</returns>
        public List<Provincia> DataSetToProvincias(DataSet data)
        {
            List<Provincia> provincias = new List<Provincia>();
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

        public List<EstadoEnvio> DataSetToEstado(DataSet data)
        {
            List<EstadoEnvio> estados = new List<EstadoEnvio>();
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
    }
}