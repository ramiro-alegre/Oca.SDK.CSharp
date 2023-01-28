<div align="center">
    <h1>OCA SDK</h1>
    Este paquete no es oficial de OCA y no tiene ánimo de lucro. Es un proyecto personal creado para hacer más fácil la integración con OCA y una alternativa al WSDL.
</div> <br/>

[![Nuget](https://img.shields.io/nuget/v/Oca.SDK)](https://www.nuget.org/packages/Oca.SDK)

## Descripción

Para comenzar a utilizar el paquete, hay una clase llamada HttpOcaEpak que se encarga de hacer las peticiones al WebService de OCA epak.
En el caso de que se quiera utilizar este paquete apuntando al webservice de producción, se debe de ingresar el usuario, contraseña y numero de cuenta de OCA ePak, caso contrario, simplemente se debe de dejar en false el ultimo parametro. <br/>
Por el momento, no es posible utilizar async/await.

Ejemplo:
```csharp
// Ejemplo de apuntando a producción.
var ocaEpakProd = new HttpOcaEpak("usrProd", "pswProd", "123456/12", true);
// Ejemplo de apuntando a test. Las credenciales se inicializan automaticamente.
var ocaEpakTest = new HttpOcaEpak("", "", "", false);

// * Obteniendo las provincias
ResponseOca<Provincia> provincias = ocaEpakTest.GetProvincias();
Console.WriteLine(provincias.Data.Count);

// * Obteniendo las sucursales/Centros de imposición
ResponseOca<Sucursal> sucursales = ocaEpakTest.GetCentrosImposicionConServicios();
Console.WriteLine(sucursales.Data.Count);
```



