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
        httpOcaEpak = new HttpOcaEpak(false);
    }

    [TestMethod]
    public void Obtener_Sucursales_Default_Correcto()
    {
        ResponseOca<Sucursal> responseOca = httpOcaEpak.GetCentrosImposicionConServicios();
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
        Assert.AreEqual(responseOca.Success, false, "Se esperaba que la respuesta sea false");
    }

}