<div align="center">
    <h1>OCA SDK</h1>
    Este paquete no es oficial de OCA y no tiene ánimo de lucro. Es un proyecto personal que cree para hacer más fácil la integración con OCA.
</div>

## Descrición

Para comenzar a utilizar el paquete, hay una clase llamada HttpOcaEpak que se encarga de hacer las peticiones al WebService de OCA epak.
Por el momento, recibe como parametro si es producción o no (Por defecto es producción), ya que aun no hay implementaciones que requieran de usuario o contraseña.
Por el momento, no es posible utilizar async await.

Ejemplo:
```csharp
var ocaEpak = new HttpOcaEpak();

// * Obteniendo las provincias
ResponseOca<Provincia> provincias = ocaEpak.GetProvincias();
Console.WriteLine(provincias.Data.Count);

// * Obteniendo las sucursales/Centros de imposición
ResponseOca<Sucursal> sucursales = ocaEpak.GetCentrosImposicionConServicios();
Console.WriteLine(sucursales.Data.Count);
```



