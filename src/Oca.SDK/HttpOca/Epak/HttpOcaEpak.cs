using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using Oca.SDK.Entitys;
using Oca.SDK.Exceptions;
using Oca.SDK.Response;
using Oca.SDK.Services;

namespace OCA.SDK.HttpOca.Epak{
    public class HttpOcaEpak{
        /// <summary>
        /// Define qué URL se va a utilizar en la conexión al webservice de OCA.
        /// </summary>
        public readonly bool esProduccion;
        /// <summary>
        /// URL Interna que se utiliza para la conexión al webservice de OCA.
        /// </summary>
        public readonly string _url;
        private readonly WebClient wc;
        private readonly HttpOcaEpakHelper _httpOcaEpakHelper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="esProduccion">Define qué webservice se va a utilizar <br/>
        ///                            true: webservice de producción <br/>
        ///                            false: webservice de pruebas
        /// </param>
        public HttpOcaEpak(bool esProduccion = true){
            _url = esProduccion ? "http://webservice.oca.com.ar/epak_tracking/" : "http://webservice.oca.com.ar/epak_Tracking_TEST/";
            this.esProduccion = esProduccion;
            this.wc = new WebClient();
            this._httpOcaEpakHelper = new HttpOcaEpakHelper(_url);
        }
        /// <summary>
        /// Obtiene las sucursales con servicios activas al momento desde el servicio de Oca
        /// </summary>
        /// <param name="tipo">Tipo de servicio que de la sucursal que se quiera restacar, por defecto, devuelve todas</param>
        /// <param name="ConCodigosPostalesAcepta">En el caso de que sea true, rellena la lista "CodigosPostalesQueAcepta" de la clase Sucursal, caso contrario, la deja vacia y sin inicializar</param>
        /// <returns>Lista de sucursales</returns>
        public ResponseOca<Sucursal> GetCentrosImposicionConServicios(TipoServicio tipo = TipoServicio.SinFiltro, bool ConCodigosPostalesAcepta = false){
            string xmlResponse = wc.DownloadString($"{_url}Oep_TrackEPak.asmx/GetCentrosImposicionConServicios?");
            DataSet dataset = Utils.XmlUtils.ToDataSet(xmlResponse);
            List<Sucursal> sucursales = new List<Sucursal>();
            try{
                sucursales = _httpOcaEpakHelper.DataSetToSucursales(dataset, tipo, ConCodigosPostalesAcepta);
                return new ResponseOca<Sucursal>(){
                    Success = true,
                    Data = sucursales,
                    Message = "OK"
                };
            }
            catch(ListEmptyException e){
                return new ResponseOca<Sucursal>(){
                    Success = false,
                    Data = sucursales,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new ResponseOca<Sucursal>(){
                    Success = false,
                    Data = sucursales,
                    Message = "Error no controlado: " + e.Message
                };
            }
        }
        /// <summary>
        /// Obtiene las provincias
        /// </summary>
        /// <returns>Lista de provincioas</returns>
        public ResponseOca<Provincia> GetProvincias()
        {
            string xmlResponse = wc.DownloadString($"{_url}Oep_TrackEPak.asmx/GetProvincias");
            DataSet dataset = Utils.XmlUtils.ToDataSet(xmlResponse);
            List<Provincia> provincias = new List<Provincia>();
            try{
                provincias = _httpOcaEpakHelper.DataSetToProvincias(dataset);
                return new ResponseOca<Provincia>(){
                    Success = true,
                    Data = provincias,
                    Message = "OK"
                };
            }
            catch(ListEmptyException e){
                return new ResponseOca<Provincia>(){
                    Success = false,
                    Data = provincias,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new ResponseOca<Provincia>(){
                    Success = false,
                    Data = provincias,
                    Message = "Error no controlado: " + e.Message
                };
            }
        }
        /// <summary>
        /// Obtiene todos los estados que tuvo el envío.
        /// </summary>
        /// <param name="numeroEnvio">Numero de envio dado por Oca</param>
        /// <returns>Lista con todos los estados</returns>
        public ResponseOca<EstadoEnvio> TrackingPieza(string numeroEnvio){
            string xmlResponse = wc.DownloadString($"{_url}Oep_TrackEPak.asmx/Tracking_Pieza?NroDocumentoCliente=0&CUIT=0&Pieza={numeroEnvio}");
            DataSet dataset = Utils.XmlUtils.ToDataSet(xmlResponse);
            List<EstadoEnvio> estados = new List<EstadoEnvio>();
            try{
                estados = _httpOcaEpakHelper.DataSetToEstado(dataset);
                return new ResponseOca<EstadoEnvio>(){
                    Success = true,
                    Data = estados,
                    Message = "OK"
                };
            }
            catch(ListEmptyException e){
                return new ResponseOca<EstadoEnvio>(){
                    Success = false,
                    Data = estados,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new ResponseOca<EstadoEnvio>(){
                    Success = false,
                    Data = estados,
                    Message = "Error no controlado: " + e.Message
                };
            }
        }
    }
}