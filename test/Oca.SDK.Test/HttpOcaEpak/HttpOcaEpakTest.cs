using Oca.SDK.Entitys;
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
        List<Sucursal> sucursales = httpOcaEpak.GetCentrosImposicionConServicios();
        Assert.AreNotEqual(sucursales.Count, 0, "No se obtuvieron sucursales");
    }

    [TestMethod]
    public void Obtener_Provincias_Correcto(){
        List<Provincia> provincias = httpOcaEpak.GetProvincias();
        Assert.AreNotEqual(provincias.Count, 0, "No se obtuvieron provincias");
    }

    [TestMethod]
    public void Obtener_Estados_Envio_Correcto(){
                                                        // * Código de seguimiento publicado en internet.
        List<EstadoEnvio> estados = httpOcaEpak.TrackingPieza("3867500000001725327");
        Assert.AreNotEqual(estados.Count, 0, "No se obtuvieron estados");
    }

    // [TestMethod]
    // public void Obtener_Estados_Envio_Falla_NroEnvio_No_Existe(){
    //     List<EstadoEnvio> estados = httpOcaEpak.TrackingPieza("2231300000000004007");
    //     Assert.AreNotEqual(estados.Count, 0, "No se obtuvieron estados");
    // }

}