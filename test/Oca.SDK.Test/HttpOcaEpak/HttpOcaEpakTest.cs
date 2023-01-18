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
}