using System.Collections.Generic;
using System.Data;
using System.Net;
using Oca.SDK.Entitys;
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
        public List<Sucursal> GetCentrosImposicionConServicios(TipoServicio tipo = TipoServicio.SinFiltro, bool ConCodigosPostalesAcepta = false){
            string xmlResponse = wc.DownloadString($"{_url}Oep_TrackEPak.asmx/GetCentrosImposicionConServicios?");
            DataSet dataset = Utils.XmlUtils.ToDataSet(xmlResponse);
            return _httpOcaEpakHelper.DataSetToSucursales(dataset, tipo, ConCodigosPostalesAcepta);
        }
        /// <summary>
        /// Obtiene las provincias
        /// </summary>
        /// <returns>Lista de provincioas</returns>
        public List<Provincia> GetProvincias()
        {
            string xmlResponse = wc.DownloadString($"{_url}Oep_TrackEPak.asmx/GetProvincias");
            DataSet dataset = Utils.XmlUtils.ToDataSet(xmlResponse);
            return _httpOcaEpakHelper.DataSetToProvincias(dataset);
        }


    }
}