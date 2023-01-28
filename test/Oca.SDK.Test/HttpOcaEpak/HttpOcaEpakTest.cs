using Oca.SDK.Entitys;
using Oca.SDK.Response;
using OCA.SDK.HttpOca.Epak;

namespace Oca.SDK.Test;

[TestClass]
public class HttpOcaEpakTest
{
    private readonly HttpOcaEpak httpOcaEpak;
    public HttpOcaEpakTest()
    {
        httpOcaEpak = new HttpOcaEpak("","","",false);
    }

    [TestMethod]
    public void Obtener_Sucursales_Default_Correcto()
    {
        ResponseOca<Sucursal> responseOca = httpOcaEpak.GetCentrosImposicionConServicios();
        Assert.AreNotEqual(responseOca.Data.Count, 0, "Se esperaba que se devolvieran sucursales");
    }

    [TestMethod]
    public void Obtener_Sucursales_Con_Filtro_Correcto()
    {
        ResponseOca<Sucursal> responseOca = httpOcaEpak.GetCentrosImposicionConServicios(TipoServicio.EntregaDePaquetes);
        Assert.AreNotEqual(responseOca.Data.Count, 0, "Se esperaba que se devolvieran sucursales");
    }

    [TestMethod]
    public void Obtener_Sucursales_Con_Codigos_Postales_Acepta_Correcto()
    {
        ResponseOca<Sucursal> responseOca = httpOcaEpak.GetCentrosImposicionConServicios(TipoServicio.SinFiltro, true);
        Assert.AreNotEqual(responseOca.Data.Count, 0, "Se esperaba que se devolvieran sucursales");
        Assert.AreNotEqual(responseOca.Data.Where(d => d.CodigosPostalesQueAcepta.Count == 0).Count(), 0, "Se esperaba que se devolvieran sucursales con codigos postales");
    }


    [TestMethod]
    public void Obtener_Sucursales_ByCP_Default_Correcto(){
        ResponseOca<Sucursal> responseOca = httpOcaEpak.GetCentrosImposicionConServiciosByCP(1617);
        Assert.AreNotEqual(responseOca.Data.Count, 0, "Se esperaba que se devolvieran sucursales");
    }

    [TestMethod]
    public void Obtener_Provincias_Correcto(){
        ResponseOca<Provincia> responseOca = httpOcaEpak.GetProvincias();
        Assert.AreNotEqual(responseOca.Data.Count, 0, "Se espera que se devolvieran provincias");
    }

    [TestMethod]
    public void Obtener_Estados_Envio_Correcto(){
                                                        // * Código de seguimiento publicado en internet.
        ResponseOca<EstadoEnvio> responseOca = httpOcaEpak.TrackingPieza("3867500000001725327");
        Assert.AreNotEqual(responseOca.Data.Count, 0, "Se esperaba que se devolvieran estados del envio");
    }

    [TestMethod]
    public void Obtener_Estados_Envio_Falla_NroEnvio_No_Existe(){
        ResponseOca<EstadoEnvio> responseOca = httpOcaEpak.TrackingPieza("2231300000000004007");
        Assert.AreEqual(responseOca.Data.Count, 0, "Se esperaba que no se devolvieran estados");
        Assert.AreEqual(responseOca.Success, false, "Se esperaba que Sucess sea false");
    }

    [TestMethod]
    public void Obtener_Codigos_Postales_Por_Centro_Imposicion_Correcto(){
        ResponseOca<string> responseOca = httpOcaEpak.GetCodigosPostalesXCentroImposicion(146);
        Assert.AreNotEqual(responseOca.Data.Count, 0, "Se esperaba que se devolvieran codigos postales");
    }

    [TestMethod]
    public void Generar_OR_Correcto_Generando_Xml(){
        // * Este XML (Por la operativa), es de puerta_a_puerta, y esto se puede deducir ya que el origen tiene "idcentroimposicionorigen" en 0,
        // * y el destino tiene el "idci" en 0, indicando de que no se especifica sucursal de origen ni de destino.

        string xmlTemporal = @"<?xml version=""1.0"" encoding=""iso-8859-1"" standalone=""yes""?> 
        <ROWS> 
            <cabecera ver=""2.0"" nrocuenta=""111757/001""/> 
            <origenes> 
                <origen calle=""La Rioja"" nro=""300"" piso="""" depto="""" cp=""1215"" localidad=""CAPITAL FEDERAL"" provincia=""CAPITAL FEDERAL"" contacto="""" email=""test@oca.com.ar"" solicitante="""" observaciones="""" centrocosto="""" idfranjahoraria=""1"" idcentroimposicionorigen=""0"" fecha=""20151015""> 
                    <envios> 
                        <envio idoperativa=""64665"" nroremito=""Envio1""> 
                            <destinatario apellido=""Fernandez"" nombre=""Martin"" calle=""BALCARCE"" nro=""50"" piso="""" depto="""" localidad=""CAPITAL FEDERAL"" provincia=""CAPITAL FEDERAL"" cp=""1214"" telefono=""49569622"" email=""test@oca.com.ar"" idci=""0"" celular=""1121877788"" observaciones=""Prueba""/> 
                            <paquetes> 
                                <paquete alto=""10"" ancho=""10"" largo=""10"" peso=""1"" valor=""10"" cant=""3"" /> 
                            </paquetes> 
                        </envio> 
                    </envios> 
                </origen> 
            </origenes> 
        </ROWS>";
        ResponseOca<OrdenRetiroResponse> responseOca =  httpOcaEpak.IngresoORMultiplesRetiros(xmlTemporal, true);
        Assert.AreNotEqual(responseOca.Data.Count, 0, "Se esperaba que se devolviera información de la orden de retiro");
    }
    [TestMethod]
    public void Generar_OR_Falla_Generando_Xml_Invalido(){
        // * El simple salto de linea ya genera que el XML sea invalido.
        string xmlTemporal = @"
        <?xml version=""1.0"" encoding=""iso-8859-1"" standalone=""yes""?> 
        <ROWS> 
            <cabecera ver=""2.0"" nrocuenta=""111757/001""/> 
            <origenes> 
                <origen calle=""La Rioja"" nro=""300"" piso="""" depto="""" cp=""1215"" localidad=""CAPITAL FEDERAL"" provincia=""CAPITAL FEDERAL"" contacto="""" email=""test@oca.com.ar"" solicitante="""" observaciones="""" centrocosto="""" idfranjahoraria=""1"" idcentroimposicionorigen=""0"" fecha=""20151015""> 
                    <envios> 
                        <envio idoperativa=""64665"" nroremito=""Envio1""> 
                            <destinatario apellido=""Fernandez"" nombre=""Martin"" calle=""BALCARCE"" nro=""50"" piso="""" depto="""" localidad=""CAPITAL FEDERAL"" provincia=""CAPITAL FEDERAL"" cp=""1214"" telefono=""49569622"" email=""test@oca.com.ar"" idci=""0"" celular=""1121877788"" observaciones=""Prueba""/> 
                            <paquetes> 
                                <paquete alto=""10"" ancho=""10"" largo=""10"" peso=""1"" valor=""10"" cant=""3"" /> 
                            </paquetes> 
                        </envio> 
                    </envios> 
                </origen> 
            </origenes> 
        </ROWS>";
        ResponseOca<OrdenRetiroResponse> responseOca =  httpOcaEpak.IngresoORMultiplesRetiros(xmlTemporal, true);
        Assert.AreEqual(responseOca.Data.Count, 0, "Se esperaba que no se devolviera información de la orden de retiro");
    }
    [TestMethod]
    public void Generar_OR_Correcto_Con_Clases(){
        OrigenOca origen = new OrigenOca(){
            Calle = "La Rioja",
            Numero = 300,
            Piso = "",
            Depto = "",
            CodigoPostal = 1215,
            Localidad = "CAPITAL FEDERAL",
            Provincia = "CAPITAL FEDERAL",
            Contacto = "",
            Email = "",
            Solicitante = "",
            Observaciones = "",
            CentroCosto = "",
            FranjaHoraria = Enums.FrajaHoraria.De_8_a_17,
            IdCentroImposicionOrigen = 0,
            Fecha = DateTime.Now
        };
        Envio envio = new Envio(){
            IdOperativa = ((int)Enums.OperativasTest.Puerta_a_puerta).ToString(),
            NroRemito = "Envio1"
        };
        Destinatario destinatario = new Destinatario(){
            Apellido = "Fernandez",
            Nombre = "Martin",
            Calle = "BALCARCE",
            Numero = 50,
            Piso = "",
            Depto = "",
            Localidad = "CAPITAL FEDERAL",
            Provincia = "CAPITAL FEDERAL",
            CodigoPostal = 1214,
            Telefono = "49569622",
            Email = "",
            IdCentroImposicionDestino = 0,
            Celular = "1121877788",
            Observaciones = "Prueba"
        };
        
        List<Paquete> paquetes = new List<Paquete>();
        paquetes.Add(new Paquete(){
            Alto = 10,
            Ancho = 10,
            Largo = 10,
            Peso = 1,
            Valor = 10,
            Cantidad = 1
        });
        
        List<EnvioPaquete> envioPaquetes = new List<EnvioPaquete>();
        envioPaquetes.Add(new EnvioPaquete(){
            Envio = envio,
            Destinatario = destinatario,
            Paquetes = paquetes
        });
        OrdenRetiroDatos datos = new OrdenRetiroDatos(origen, envioPaquetes);
        ResponseOca<OrdenRetiroResponse> responseOca = httpOcaEpak.IngresoORMultiplesRetiros(datos, true);
        Assert.AreNotEqual(responseOca.Data, 0, "Se esperaba que se devolviera información de la orden de retiro");
    }
}