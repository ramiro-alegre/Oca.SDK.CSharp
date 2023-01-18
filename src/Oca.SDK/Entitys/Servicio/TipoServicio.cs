namespace Oca.SDK.Entitys{
    /// <summary>
    /// Diferentes tipos de servicio que puede tener una sucursal de OCA.
    /// </summary>
    public enum TipoServicio
    {
        /// <summary>
        /// Retorna todas las sucursales, sin importar qué servicio ofrezca
        /// </summary>
        SinFiltro = 0,
        /// <summary>
        /// La sucursal puede seleccionarse como origen de un envío.
        /// </summary>
        AdmisionDePaquetes = 1,
        /// <summary>
        /// La sucursal puede seleccionarse como destino de un envío.
        /// </summary>
        EntregaDePaquetes = 2,
        /// <summary>
        /// Sin descripción
        /// </summary>
        VentaEstampillas = 3
    }
}