using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
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
        public readonly string url;
        private readonly HttpOcaEpakHelper _httpOcaEpakHelper;
        private readonly string _usr;
        private readonly string _psw;
        private readonly string _nroCuenta;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usr">Usuario de Oca ePak</param>
        /// <param name="pws">Contraseña de Oca ePak</param>
        /// <param name="nroCuenta">Numero de cuenta de Oca ePak</param>
        /// <param name="esProduccion">True si es producción, caso contrario, se usa la URL de testing. <br/>
        /// En el caso de que se envie false, automaticamente se rellenan los demás valores con las credenciales de TEST</param>
        public HttpOcaEpak(string usr, string pws, string nroCuenta, bool esProduccion = true){
            if(esProduccion){
                this._usr = usr;
                this._psw = pws;
                this._nroCuenta = nroCuenta;
                this.url = "http://webservice.oca.com.ar/epak_tracking/";
            }else{
                this._usr = "test@oca.com.ar";
                this._psw = "123456";
                this._nroCuenta = "111757/001";
                this.url = "http://webservice.oca.com.ar/epak_Tracking_TEST/";
            }
            this._httpOcaEpakHelper = new HttpOcaEpakHelper(url);
        }
        /// <summary>
        /// Obtiene las sucursales con servicios activas al momento desde el servicio de Oca
        /// </summary>
        /// <param name="tipo">Tipo de servicio que de la sucursal que se quiera restacar, por defecto, devuelve todas</param>
        /// <param name="ConCodigosPostalesAcepta">En el caso de que sea true, rellena la lista "CodigosPostalesQueAcepta" de la clase Sucursal, caso contrario, la deja vacia y sin inicializar. <br/>
        /// La información que se obtiene es la misma que retorna <see href="GetCodigosPostalesXCentroImposicion"/></param>
        /// <returns>Lista de sucursales</returns>
        public ResponseListOca<Sucursal> GetCentrosImposicionConServicios(TipoServicio tipo = TipoServicio.SinFiltro, bool ConCodigosPostalesAcepta = false){
            string xmlResponse = "";
            using(WebClient wc = new WebClient()){
                xmlResponse = wc.DownloadString($"{url}Oep_TrackEPak.asmx/GetCentrosImposicionConServicios?");
            }
            DataSet dataset = Utils.XmlUtils.ToDataSet(xmlResponse);
            List<Sucursal> sucursales = new List<Sucursal>();
            try{
                sucursales = _httpOcaEpakHelper.DataSetToSucursales(dataset, tipo, ConCodigosPostalesAcepta);
                return new ResponseListOca<Sucursal>(){
                    Success = true,
                    Data = sucursales,
                    Message = "OK"
                };
            }
            catch(ListEmptyException e){
                return new ResponseListOca<Sucursal>(){
                    Success = false,
                    Data = sucursales,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new ResponseListOca<Sucursal>(){
                    Success = false,
                    Data = sucursales,
                    Message = "Error no controlado: " + e.Message
                };
            }
        }
        /// <summary>
        /// Obtiene las sucursales con servicios activas al momento que acepten el código postal enviado, desde el servicio de Oca.
        /// </summary>
        /// <param name="codigoPostal">Código postal a buscar</param>
        /// <param name="tipo">Tipo de servicio que de la sucursal que se quiera restacar, por defecto, devuelve todas</param>
        /// <param name="ConCodigosPostalesAcepta">En el caso de que sea true, rellena la lista "CodigosPostalesQueAcepta" de la clase Sucursal, caso contrario, la deja vacia y sin inicializar. <br/>
        /// La información que se obtiene es la misma que retorna <see href="GetCodigosPostalesXCentroImposicion"/></param>
        /// <returns></returns>
        public ResponseListOca<Sucursal> GetCentrosImposicionConServiciosByCP(int codigoPostal, TipoServicio tipo = TipoServicio.SinFiltro, bool ConCodigosPostalesAcepta = false){
            if(codigoPostal < 1000 || codigoPostal > 9999)
                return new ResponseListOca<Sucursal>(){
                    Success = false,
                    Data = new List<Sucursal>(),
                    Message = "El código postal debe ser un número de 4 dígitos"
                };

            string xmlResponse = "";
            using(WebClient wc = new WebClient()){
                xmlResponse = wc.DownloadString($"{url}Oep_TrackEPak.asmx/GetCentrosImposicionConServiciosByCP?CodigoPostal={codigoPostal}");
            }
            DataSet dataset = Utils.XmlUtils.ToDataSet(xmlResponse);
            List<Sucursal> sucursales = new List<Sucursal>();
            try{
                sucursales = _httpOcaEpakHelper.DataSetToSucursales(dataset, tipo, ConCodigosPostalesAcepta);
                return new ResponseListOca<Sucursal>(){
                    Success = true,
                    Data = sucursales,
                    Message = "OK"
                };
            }
            catch(ListEmptyException e){
                return new ResponseListOca<Sucursal>(){
                    Success = false,
                    Data = sucursales,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new ResponseListOca<Sucursal>(){
                    Success = false,
                    Data = sucursales,
                    Message = "Error no controlado: " + e.Message
                };
            }
        }
        /// <summary>
        /// Obtiene los códigos postales que acepta el centro de imposición enviado. <br/>
        /// Es decir, el/los códigos postales que se pueden enviar a esa sucursal.
        /// </summary>
        /// <param name="idCentroImposicion">Id del centro de imposición a buscar</param>
        /// <returns></returns>
        public ResponseListOca<string> GetCodigosPostalesXCentroImposicion(int idCentroImposicion){
            if(idCentroImposicion < 1)
                return new ResponseListOca<string>(){
                    Success = false,
                    Data = new List<string>(),
                    Message = "El id del centro de imposición debe ser un número mayor a 0"
                };

            string xmlResponse = "";
            using(WebClient wc = new WebClient()){
                xmlResponse = wc.DownloadString($"{url}Oep_TrackEPak.asmx/GetCodigosPostalesXCentroImposicion?IdCentroImposicion={idCentroImposicion}");
            }
            DataSet dataset = Utils.XmlUtils.ToDataSet(xmlResponse);
            List<string> codigosPostales = new List<string>();
            try{
                codigosPostales = _httpOcaEpakHelper.DataSetToCodigosPostales(dataset);
                return new ResponseListOca<string>(){
                    Success = true,
                    Data = codigosPostales,
                    Message = "OK"
                };
            }
            catch(ListEmptyException e){
                return new ResponseListOca<string>(){
                    Success = false,
                    Data = codigosPostales,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new ResponseListOca<string>(){
                    Success = false,
                    Data = codigosPostales,
                    Message = "Error no controlado: " + e.Message
                };
            }
        }
        /// <summary>
        /// Obtiene las provincias
        /// </summary>
        /// <returns>Lista de provincioas</returns>
        public ResponseListOca<Provincia> GetProvincias()
        {
            string xmlResponse = "";
            using(WebClient wc = new WebClient()){
                xmlResponse = wc.DownloadString($"{url}Oep_TrackEPak.asmx/GetProvincias");
            }
            DataSet dataset = Utils.XmlUtils.ToDataSet(xmlResponse);
            List<Provincia> provincias = new List<Provincia>();
            try{
                provincias = _httpOcaEpakHelper.DataSetToProvincias(dataset);
                return new ResponseListOca<Provincia>(){
                    Success = true,
                    Data = provincias,
                    Message = "OK"
                };
            }
            catch(ListEmptyException e){
                return new ResponseListOca<Provincia>(){
                    Success = false,
                    Data = provincias,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new ResponseListOca<Provincia>(){
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
        public ResponseListOca<EstadoEnvio> TrackingPieza(string numeroEnvio){
            string xmlResponse = "";
            using(WebClient wc = new WebClient()){
                xmlResponse = wc.DownloadString($"{url}Oep_TrackEPak.asmx/Tracking_Pieza?NroDocumentoCliente=0&CUIT=0&Pieza={numeroEnvio}");
            }
            DataSet dataset = Utils.XmlUtils.ToDataSet(xmlResponse);
            List<EstadoEnvio> estados = new List<EstadoEnvio>();
            try{
                estados = _httpOcaEpakHelper.DataSetToEstado(dataset);
                return new ResponseListOca<EstadoEnvio>(){
                    Success = true,
                    Data = estados,
                    Message = "OK"
                };
            }
            catch(ListEmptyException e){
                return new ResponseListOca<EstadoEnvio>(){
                    Success = false,
                    Data = estados,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new ResponseListOca<EstadoEnvio>(){
                    Success = false,
                    Data = estados,
                    Message = "Error no controlado: " + e.Message
                };
            }
        }
        /// <summary>
        /// Genera la orden de retiro. Recibe el XML en el caso de que se quiera generar de forma externa. <br/>
        /// En el caso de que confirmarRetiro sea false, no va a devolver nada en la data, solo una lista vacia. Para más información, ver el parametro.
        /// </summary>
        /// <param name="xmlDatos">Xml generado</param>
        /// <param name="confirmarRetiro">En el caso de que sea true, la orden de retiro va a ser confirmada y se va devolver información de la misma. 
        /// Caso contrario, la lista va a volver vacia y el envio va a quedar a la espera de su confirmación.</param>
        /// <returns></returns>
        public ResponseSingleOca<OrdenRetiroResponse> IngresoORMultiplesRetiros(string xmlDatos, bool confirmarRetiro){
            string xmlResponse = "";
            using(WebClient wc = new WebClient()){
                string parametros = $"usr={this._usr}&psw={this._psw}&xml_Datos={xmlDatos}&ConfirmarRetiro={confirmarRetiro.ToString()}&ArchivoCliente={""}&ArchivoProceso={""}";
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                xmlResponse = wc.UploadString($"{url}Oep_TrackEPak.asmx/IngresoORMultiplesRetiros", parametros);
            }
            DataSet dataset = Utils.XmlUtils.ToDataSet(xmlResponse);
            OrdenRetiroResponse? orden = null;
            try{
                orden = _httpOcaEpakHelper.DataSetToOrdenRetiroResponse(dataset);
                return new ResponseSingleOca<OrdenRetiroResponse>(){
                    Success = true,
                    Data = orden,
                    Message = (orden != null ? "OK" : "Orden de retiro generada en espera de su confirmación")
                };
            }
            catch(ListEmptyException e){
                return new ResponseSingleOca<OrdenRetiroResponse>(){
                    Success = false,
                    Data = orden,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new ResponseSingleOca<OrdenRetiroResponse>(){
                    Success = false,
                    Data = orden,
                    Message = "Error no controlado: " + e.Message
                };
            }
        }
        /// <summary>
        /// Genera la orden de retiro. <br/>
        /// En el caso de que confirmarRetiro sea false, no va a devolver nada en la data, solo una lista vacia. Para más información, ver el parametro.
        /// </summary>
        /// <param name="datos">Datos para generar la orden de retiro</param>
        /// <param name="confirmarRetiro">En el caso de que sea true, la orden de retiro va a ser confirmada y se va devolver información de la misma. 
        /// Caso contrario, la lista va a volver vacia y el envio va a quedar a la espera de su confirmación.</param>
        /// <returns></returns>
        public ResponseSingleOca<OrdenRetiroResponse> IngresoORMultiplesRetiros(OrdenRetiroDatos datos, bool confirmarRetiro){
            string xmlDatos = _httpOcaEpakHelper.OrdenRetiroDatosToXml(datos, this._nroCuenta);
            return this.IngresoORMultiplesRetiros(xmlDatos, confirmarRetiro);
        }
        
    }
}